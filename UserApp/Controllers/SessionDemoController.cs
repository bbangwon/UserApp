using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using UserApp.Extensions;

namespace UserApp.Controllers
{
    public class SessionDemoController : Controller
    {
        public IActionResult Index()
        {
            //세션 저장
            HttpContext.Session.SetString("username", "Green");

            return View();
        }

        public IActionResult GetSession()
        {
            //세션 읽기
            ViewBag.username = HttpContext.Session.GetString("username");

            return View();
        }

        /// <summary>
        /// 세션에 날짜값 저장
        /// </summary>
        /// <returns></returns>
        public IActionResult SetDate()
        {
            HttpContext.Session.Set("NowDate", DateTime.Now);
            return RedirectToAction(nameof(GetDate));
        }

        /// <summary>
        /// 세션에 날짜값 읽어오기
        /// </summary>
        /// <returns></returns>
        public IActionResult GetDate()
        {
            var date = HttpContext.Session.Get<DateTime>("NowDate");
            var sessionTime = date.TimeOfDay.ToString();
            var currentTime = DateTime.Now.TimeOfDay.ToString();

            return Content($"현재 시간: {currentTime} 세션에 저장되어 있는 시간: {sessionTime}");
        }
    }
}
