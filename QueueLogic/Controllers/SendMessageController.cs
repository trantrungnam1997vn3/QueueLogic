using QueueLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace QueueLogic.Controllers
{
    public class SendMessageController : ApiController
    {
        /// <summary>
        /// This function send message with Sync with logic queue, identity by user 
        /// </summary>
        ///
        ///<param name="id">1234</param> <note> Id user or something to identified user queue.</note>
        /// <param name="mss">Nam</param> <note>Message get from client.</note>
        /// <returns></returns>

        [HttpGet]
        public string SendMessageWithSyn(int id, string mss)
        {
            string result = "";
            Action<String> a = (message) =>
            {
                QueueResponse.AddMssResponseToQueue(123, message);
            };
            MessageQueue<String> messageQueue = new MessageQueue<String>(a);
            Thread.Sleep(1000);
            messageQueue.EnqueueAMessage(mss);
            QueueResponse.GetResultById(123, out result);
            return result;
        }

        [HttpGet]
        public IHttpActionResult SendMessageWithTimeout(int id, string mss)
        {
            Console.WriteLine("Breakfast is ready!");
            string result = "";
            SupportTest supportTest = new SupportTest();
            bool statusTimeout = supportTest.ImpatientMethod();
            if (statusTimeout)
            {
                return Ok("Done");
            }
            return BadRequest("Failed");
        }

        [HttpGet]
        public string GetMessageAfter2Minutes()
        {
            Thread.Sleep(2000);
            return "Done";
        }

    }
}
