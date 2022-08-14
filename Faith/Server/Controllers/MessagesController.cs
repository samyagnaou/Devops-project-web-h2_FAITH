using Faith.Core.Interfaces;
using Faith.Shared;
using Faith.Shared.Models;
using Faith.Shared.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Faith.Server.Controllers
{
    [Authorize(Roles = Roles.Student)]
    public class MessagesController : ApiControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMessageService _messageService;

        public MessagesController(
            IMessageService messageService,
            UserManager<IdentityUser> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<MessageDTO>> GetAllMessages()
        {
            var messages = await _messageService
                .GetAllMessages(User.Identity!.Name!);
            return messages.Select(m =>
                new MessageDTO
                {
                    Text = m.Text,
                    ImageUrl = m.ImageUrl,
                    CreatedBy = User!.Identity!.Name!,
                    CreatedAt = m.CreatedAt
                });
        }


        [HttpPost]
        public async Task<IActionResult> PostMessage(PostMessageRequest request)
        {
            var isMessageSent = await _messageService
                .PostMessage(User.Identity!.Name!, request.Text, request.ImageUrl);
            if (isMessageSent)
                return Ok();
            return UnprocessableEntity();
        }
    }
}
