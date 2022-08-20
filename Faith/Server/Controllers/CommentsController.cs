using Faith.Core.Interfaces;
using Faith.Shared.Constants;
using Faith.Shared.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Faith.Server.Controllers
{
    [Authorize(Roles = $"{Roles.Mentor},{Roles.Student}")]
    public class CommentsController : ApiControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentToPost([FromBody] AddCommentRequest req)
        {
            var isCommentAdded = await _commentService
                .AddCommentToPost(User!.Identity!.Name!, req.MessageId, req.Text);
            if (!isCommentAdded)
                return UnprocessableEntity();

            return Ok();
        }
    }
}