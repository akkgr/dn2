using cinnamon.api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cinnamon.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EnumController : Controller
    {
        [Route("address")]
        [HttpGet]
        public IActionResult GetAddress()
        {
            var enumVals = new List<object>();

            foreach (var item in Enum.GetValues(typeof(AddressType)))
            {
                enumVals.Add(new
                {
                    key = (int)item,
                    value = item.ToString()
                });
            }

            return Ok(enumVals);
        }

        [Route("personinfo")]
        [HttpGet]
        public IActionResult GetPersonInfo()
        {
            var enumVals = new List<object>();

            foreach (var item in Enum.GetValues(typeof(PersonInfoType)))
            {
                enumVals.Add(new
                {
                    key = (int)item,
                    value = item.ToString()
                });
            }

            return Ok(enumVals);
        }

        [Route("person")]
        [HttpGet]
        public IActionResult GetPerson()
        {
            var enumVals = new List<object>();

            foreach (var item in Enum.GetValues(typeof(PersonType)))
            {
                enumVals.Add(new
                {
                    key = (int)item,
                    value = item.ToString()
                });
            }

            return Ok(enumVals);
        }

        [Route("phone")]
        [HttpGet]
        public IActionResult GetPhone()
        {
            var enumVals = new List<object>();

            foreach (var item in Enum.GetValues(typeof(PhoneType)))
            {
                enumVals.Add(new
                {
                    key = (int)item,
                    value = item.ToString()
                });
            }

            return Ok(enumVals);
        }

        [Route("repairstatus")]
        [HttpGet]
        public IActionResult GetRepairStatus()
        {
            var enumVals = new List<object>();

            foreach (var item in Enum.GetValues(typeof(RepairStatus)))
            {
                enumVals.Add(new
                {
                    key = (int)item,
                    value = item.ToString()
                });
            }

            return Ok(enumVals);
        }

        [Route("requeststatus")]
        [HttpGet]
        public IActionResult GetRequestStatus()
        {
            var enumVals = new List<object>();

            foreach (var item in Enum.GetValues(typeof(RequestStatus)))
            {
                enumVals.Add(new
                {
                    key = (int)item,
                    value = item.ToString()
                });
            }

            return Ok(enumVals);
        }
    }
}
