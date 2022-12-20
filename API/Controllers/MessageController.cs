using AutoMapper;
using DatingApp.BLL.MessageManagement;
using DatingApp.DAL.DTO.Message;
using DatingApp.DAL.Extensions;
using DatingApp.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.FrontEndAPI.Controllers
{
    [Authorize]
    public class MessageController : BaseApiController
    {
        private readonly IMessageService _msgService;
        private readonly IMapper _mapper;

        public MessageController(IMessageService msgService, IMapper mapper)
        {
            _msgService ??= msgService;
            _mapper ??= mapper;
        }

        [HttpPost("create-message")]
        public async Task<ActionResult> CreateMessage(CreateMessageDto dto)
        {
            var message = _mapper.Map<Message>(dto);
            message.SenderId = User.GetUserId();
            await _msgService.CreateMessageAsync(message);
            var result = _mapper.Map<GetMessageDto>(message);
            return new OkObjectResult(result);
        }

        [HttpGet("{groupId}/get-message")]
        public async Task<ActionResult> GetMessage(string groupId)
        {
            var userId = User.GetUserId();
            var messages = await _msgService.GetMessageByGroupId(groupId, userId);
            return new OkObjectResult(_mapper.Map<List<GetMessageDto>>(messages));
        }

        [HttpDelete("{groupId}/{messageId}/delete-message")]
        public async Task<ActionResult> DeleteMessage(string groupId, string messageId)
        {
            var userId = User.GetUserId();
            var dto = new DeleteMessageDto { 
                MessageId = messageId, 
                GroupId = groupId, 
                UserId = userId
            };
            await _msgService.DeleteMessage(dto);
            return new OkResult();
        }
    }
}
