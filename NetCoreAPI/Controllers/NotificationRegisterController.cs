using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using NetCoreAPI.Data.Interfaces;

namespace NetCoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Notifications")]
    public class NotificationRegisterController : Controller
    {
        NotificationHubClient _hub;
        public NotificationRegisterController(INotificationHub hub)
        {
            _hub = hub.GetHub();
        }

        public class DevicecRegistration
        {
            public string Platform { get; set; }
            public string Handle { get; set; }
            public string[] Tags { get; set; }
        }

        //Post api/register
        [HttpPost]
        public async Task<string> Post()
        {
            return await _hub.CreateRegistrationIdAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] DevicecRegistration deviceUpdate)
        {
            RegistrationDescription registration = null;
            switch (deviceUpdate.Platform)
            {
                case "mpns":
                    registration = new MpnsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "wns":
                    registration = new WindowsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "apns":
                    registration = new AppleRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "gcm":
                    registration = new GcmRegistrationDescription(deviceUpdate.Handle);
                    break;
                default:
                    return BadRequest(deviceUpdate);
            }

            registration.RegistrationId = id;
            registration.Tags = new HashSet<string>(deviceUpdate.Tags);

            try
            {
                await _hub.CreateOrUpdateRegistrationAsync(registration);
            }catch(MessagingException e)
            {
                return ReturnGoneIfHubResponseIsGone(e);
            }
            return Ok(id);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _hub.DeleteRegistrationAsync(id);
            return Ok(id);
        }

        private IActionResult ReturnGoneIfHubResponseIsGone(MessagingException e)
        {
            var webex = e.InnerException as WebException;
            if(webex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = (HttpWebResponse)webex.Response;
                if(response.StatusCode == HttpStatusCode.Gone)
                {
                    return BadRequest(HttpStatusCode.Gone.ToString());
                }
            }
            return BadRequest();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}