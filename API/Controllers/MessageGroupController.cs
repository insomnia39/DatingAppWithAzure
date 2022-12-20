using AutoMapper;
using DatingApp.BLL.MessageGroupManagement;
using DatingApp.BLL.MessageManagement;
using DatingApp.BLL.UserManagement;
using DatingApp.DAL.DTO.Message;
using DatingApp.DAL.DTO.MessageGroup;
using DatingApp.DAL.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.FrontEndAPI.Controllers
{
    [Authorize]
    public class MessageGroupController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IMessageGroupService _msgGrpService;
        private readonly IUserService _userService;

        public MessageGroupController(IMapper mapper, IMessageGroupService msgGrpService, IUserService userService)
        {
            _mapper ??= mapper;
            _msgGrpService ??= msgGrpService;
            _userService ??= userService;
        }

        [HttpGet("get-messagegroup")]
        public async Task<ActionResult> GetMessageGroup()
        {
            var userId = User.GetUserId();
            var msgGrp = await _msgGrpService.GetMessageGroupAsync(userId);
            var dto = _mapper.Map<List<GetMessageGroupDto>>(msgGrp);
            var listUserId = dto.Select(s => s.ReceiverId).Distinct().ToList();
            var listUser = await _userService.GetUserByIdAsync(listUserId);
            foreach (var data in dto)
            {
                var user = listUser.Find(p => p.Id == data.ReceiverId);
                data.ReceiverPhotoUrl = user.GetPhotoUrl();
                data.ReceiverUsername = user.Username;
            }
            return new OkObjectResult(dto);
        }
    }
}
