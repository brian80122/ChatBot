using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using FriendChatBot.Models.Entities;
using FriendChatBot.Repositories;
using isRock.LineBot;
using Microsoft.AspNetCore.Mvc;

namespace FriendChatBot.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {

        private Bot Bot { get; }

        public ChatBotController(Bot bot)
        {
            Bot = bot;
        }


        [HttpGet("Query")]
        public IActionResult Query([FromQuery] string key)
        {
            var found = new KeywordRepository().QueryByKey(key);
            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        [HttpPost("Insert")]
        public IActionResult Insert(Keyword keyword)
        {
            var repo = new KeywordRepository();
            return Ok(repo.Insert(keyword));
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(string key)
        {
            var repo = new KeywordRepository();
            return Ok(repo.Delete(key));
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {

            var receiveData = "";
            var returnData = "";
            using (var reader = new StreamReader(Request.Body))
            {
               receiveData = await reader.ReadToEndAsync();
            }

            if (string.IsNullOrEmpty(receiveData))
            {
                return Ok();
            }
            var receivedMessage = Utility.Parsing(receiveData);
            var keyword = receivedMessage.events[0].message.text;
            var repo = new KeywordRepository();
            returnData = repo.QueryByKey(keyword)?.Message;

            if (!string.IsNullOrEmpty(returnData))
            {
                Bot.ReplyMessage(receivedMessage.events[0].replyToken, returnData);
                return Ok();
            }

            if (keyword.StartsWith("學"))
            {
                var splitStrings = keyword.Split(" ");
                if (splitStrings.Length < 3)
                {
                    Bot.ReplyMessage(receivedMessage.events[0].replyToken, "請用\"學 關鍵字 訊息\" 的格式學習關鍵字");
                }
                else
                {
                    repo.Insert(new Keyword { Key = splitStrings[1], Message = splitStrings[2] });
                    Bot.ReplyMessage(receivedMessage.events[0].replyToken, $"已學習 {splitStrings[1]}");
                }
            }
            else if (keyword.StartsWith("忘記"))
            {
                var splitStrings = keyword.Split(" ");
                if (splitStrings.Length < 2)
                {
                    Bot.ReplyMessage(receivedMessage.events[0].replyToken, "請用\"忘記 關鍵字\" 的忘記關鍵字");
                }
                else
                {
                    repo.Delete(splitStrings[1]);
                    Bot.ReplyMessage(receivedMessage.events[0].replyToken, $"已忘記 {splitStrings[1]}");
                }
            }

            return Ok();
        }

        [Route("Receive"), HttpPost]
        public IActionResult Message(MessageModel model)
        {
            return Ok();
        }

    }

    public class MessageModel
    {
        public string Message { get; set; }
    }
}
