using Faith.Core.Interfaces;
using Faith.Core.Models;
using Faith.Shared.Constants;
using Faith.Shared.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Faith.Server.Controllers
{
    public class MessagesController : ApiControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(
            IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Student)]
        public async Task<IEnumerable<Message>> GetAllMessagesForAStudent()
        {
            var messages = await _messageService
                .GetAllMessagesForAStudent(User.Identity!.Name!);
            return messages;
        }

        [HttpGet("group")]
        [Authorize(Roles = Roles.Mentor)]
        public async Task<IEnumerable<Message>> GetAllMessagesInMentorGroup()
        {
            var messages = await _messageService
                .GetAllMessagesInMentorGroup(User.Identity!.Name!);
            return messages;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Student)]
        public async Task<IActionResult> PostAMessage(PostMessageRequest request)
        {
            var isMessageSent = await _messageService
                .PostAMessage(User.Identity!.Name!, request.Text, request.ImageUrl);
            if (isMessageSent)
                return Ok();
            return UnprocessableEntity();
        }
    }
}