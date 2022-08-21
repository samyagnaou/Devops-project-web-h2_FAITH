using Faith.Core.Interfaces;
using Faith.Core.Models;
using Faith.Shared.Constants;
using Faith.Shared.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Faith.Shared.Models.Responses;

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
        public async Task<MentorMessagesResponse> GetAllMessagesInMentorGroup()
        {
            var (messages, archivedMessages) = await _messageService
                 .GetAllMessagesInMentorGroup(User.Identity!.Name!);
            return new MentorMessagesResponse
            {
                Messages = messages,
                ArchivedMessages = archivedMessages
            };
        }

        [HttpPost("archive")]
        [Authorize(Roles = $"{Roles.Mentor},{Roles.Student}")]
        public async Task<IActionResult> ArchiveAMessage([FromBody] int messageId)
        {
            bool isArchived = false;
            if (User.IsInRole(Roles.Mentor))
                isArchived = await _messageService
                    .ArchiveAMessageForMentor(messageId, User!.Identity!.Name!);
            else
                isArchived = await _messageService
                    .ArchiveAMessageForStudent(messageId, User!.Identity!.Name!);

            if (!isArchived)
                return UnprocessableEntity();
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Student)]
        public async Task<IActionResult> PostAMessage(PostMessageRequest request)
        {
            var isMessageSent = await _messageService
                .PostAMessage(User.Identity!.Name!, request.Text, request.ImageUrl);
            if (!isMessageSent)
                return UnprocessableEntity();
            return Ok();
        }
    }
}