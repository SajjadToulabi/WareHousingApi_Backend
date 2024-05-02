using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHousingApi.Common.PublicTools;
using WareHousingApi.DataModel.Services.Interface;
using WareHousingApi.Entities.Entities;
using WareHousingApi.Entities.Models;
using WareHousingApi.Entities;
using WareHousingApi.WebFramework.StandardApiResult;
using WareHousingApi.Common;

namespace WareHousingApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderApiController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        public ReminderApiController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet("GetReminder")]
        public ApiResponse<IEnumerable<Reminder_Tbl>> Get(string userID)
        {
            var ReminderList = _context.ReminderUW.Get().Where(c => c.UserID == userID);
            return new ApiResponse<IEnumerable<Reminder_Tbl>>
            {
                flag = true,
                StatusCode = ApiResultStatusCode.Success,
                Message = ApiResultStatusCode.Success.DisplayNameAttribute(DisplayProperty.Name),
                Data = ReminderList
            };
        }
        [HttpGet("GetReminderinday")]
        public ApiResponse<IEnumerable<Reminder_Tbl>> GetReminderinday(string userID)
        {
            var datetimenoew = ConvertDate.ConvertMiladiToShamsi(DateTime.Now, "yyyy/MM/dd");
            var dateshamsi = ConvertDate.ConvertShamsiToMiladi(datetimenoew);
            var ReminderList = _context.ReminderUW.Get().Where(c => c.UserID == userID && c.ReminderDate== dateshamsi);
            return new ApiResponse<IEnumerable<Reminder_Tbl>>
            {
                flag = true,
                StatusCode = ApiResultStatusCode.Success,
                Message = ApiResultStatusCode.Success.DisplayNameAttribute(DisplayProperty.Name),
                Data = ReminderList
            };
        }

        [HttpGet("GetById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Reminder_Tbl), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ApiResponse GetById([FromQuery] int ReminderId)
        {
            var reminder = _context.ReminderUW.GetById(ReminderId);

            if (reminder == null) return new ApiResponse
            {
                flag = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Message = ApiResultStatusCode.NotFound.DisplayNameAttribute(DisplayProperty.Name)
            };

            return new ApiResponse<Reminder_Tbl>
            {
                flag = true,
                StatusCode = ApiResultStatusCode.Success,
                Message = ApiResultStatusCode.Success.DisplayNameAttribute(DisplayProperty.Name),
                Data = reminder
            };
        }

        [HttpPost]
        [Route("CreateReminderApi")]
        public ApiResponse Create([FromForm] ReminderViewModel model)
        {
            //کنترل نال بودن اطلاعات
            if (model.ReminderTitle=="") return new ApiResponse
            {
                flag = false,
                StatusCode = ApiResultStatusCode.BadRequest,
                Message = ApiResultStatusCode.BadRequest.DisplayNameAttribute(DisplayProperty.Name)
            };
            //کنترل تکراری نبودن اطلاعات
            var getreminder = _context.ReminderUW.Get(r => r.ReminderTtile == model.ReminderTitle && r.ReminderDate.ToString() == model.ReminderDate);
            if (getreminder.Count() > 0)
            {
                //تکراری
                return new ApiResponse
                {
                    flag = false,
                    StatusCode = ApiResultStatusCode.DuplicateInformation,
                    Message = ApiResultStatusCode.DuplicateInformation.DisplayNameAttribute(DisplayProperty.Name)
                };
            }
            try
            {
                Reminder_Tbl C = new Reminder_Tbl
                {
                    ReminderTtile = model.ReminderTitle,
                    ReminderContent = model.ReminderContent,
                    IsRead = false,
                    ReminderDate = ConvertDate.ConvertShamsiToMiladi(model.ReminderDate),
                    UserID = model.UserID,
                    CreateDateTime = DateTime.Now
                };
                _context.ReminderUW.Create(C);
                _context.Save();
                return new ApiResponse<Reminder_Tbl>
                {
                    flag = true,
                    StatusCode = ApiResultStatusCode.Success,
                    Message = ApiResultStatusCode.Success.DisplayNameAttribute(DisplayProperty.Name),
                    Data = C
                };
            }
            catch (Exception)
            {
                return new ApiResponse
                {
                    flag = false,
                    StatusCode = ApiResultStatusCode.ServerError,
                    Message = ApiResultStatusCode.ServerError.DisplayNameAttribute(DisplayProperty.Name)
                };
            }
        }

        [HttpPut("{id}")]
        [Route("UpdateReminder")]
        [ProducesResponseType(typeof(Reminder_Tbl), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ApiResponse Update([FromForm] RemindereditViewModel model)
        {
            if (model.reminderIDE == 0) return new ApiResponse
            {
                flag = false,
                StatusCode = ApiResultStatusCode.BadRequest,
                Message = ApiResultStatusCode.BadRequest.DisplayNameAttribute(DisplayProperty.Name)
            };

            if (!ModelState.IsValid) return new ApiResponse
            {
                flag = false,
                StatusCode = ApiResultStatusCode.BadRequest,
                Message = ApiResultStatusCode.BadRequest.DisplayNameAttribute(DisplayProperty.Name)
            };

            var getreminder = _context.ReminderUW.GetById(model.reminderIDE);
            if (getreminder != null)
            {
                getreminder.ReminderTtile = model.ReminderTitleE;
                getreminder.ReminderDate = ConvertDate.ConvertShamsiToMiladi(model.ReminderDateE);
                getreminder.ReminderContent = model.ReminderContentE;
                getreminder.IsRead = model.isReadE;
            }
            _context.ReminderUW.Update(getreminder);
            _context.Save();
            return new ApiResponse<Reminder_Tbl>
            {
                flag = true,
                StatusCode = ApiResultStatusCode.Success,
                Message = ApiResultStatusCode.Success.DisplayNameAttribute(DisplayProperty.Name),
                Data = getreminder
            };
        }
        [HttpPut("{id}")]
        [Route("ReadReminder")]
        [ProducesResponseType(typeof(Reminder_Tbl), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ApiResponse ReadReminder([FromQuery] int ReminderId)
        {
            if (ReminderId == 0) return new ApiResponse
            {
                flag = false,
                StatusCode = ApiResultStatusCode.BadRequest,
                Message = ApiResultStatusCode.BadRequest.DisplayNameAttribute(DisplayProperty.Name)
            };

            if (!ModelState.IsValid) return new ApiResponse
            {
                flag = false,
                StatusCode = ApiResultStatusCode.BadRequest,
                Message = ApiResultStatusCode.BadRequest.DisplayNameAttribute(DisplayProperty.Name)
            };

            var getreminder = _context.ReminderUW.GetById(ReminderId);
            if (getreminder != null)
            {
                getreminder.IsRead =true;
            }
            _context.ReminderUW.Update(getreminder);
            _context.Save();
            return new ApiResponse<Reminder_Tbl>
            {
                flag = true,
                StatusCode = ApiResultStatusCode.Success,
                Message = ApiResultStatusCode.Success.DisplayNameAttribute(DisplayProperty.Name),
                Data = getreminder
            };
        }
        [HttpDelete]
        [Route("DeleteReminder")]
        public ApiResponse DeleteReminderPost(int ReminderID)
        {
            if (ReminderID == null) return new ApiResponse
            {
                flag = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Message = ApiResultStatusCode.NotFound.DisplayNameAttribute(DisplayProperty.Name)
            };
            else
            {
                try
                {
                    _context.ReminderUW.DeleteById(ReminderID);
                    _context.Save();
                    return new ApiResponse<Reminder_Tbl>
                    {
                        flag = true,
                        StatusCode = ApiResultStatusCode.Success,
                        Message = ApiResultStatusCode.Success.DisplayNameAttribute(DisplayProperty.Name),
                    };
                }
                catch
                {
                     return new ApiResponse
                    {
                        flag = false,
                        StatusCode = ApiResultStatusCode.BadRequest,
                        Message = ApiResultStatusCode.BadRequest.DisplayNameAttribute(DisplayProperty.Name)
                    };
                }

            }

        }

    }
}
