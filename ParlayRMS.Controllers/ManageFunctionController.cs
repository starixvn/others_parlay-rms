using System.Linq;
using System.Web.Mvc;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Functions;
using ParlayRMS.Services.Users;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.ManageFunction;
using ParlayRMS.Web.Authorization;

namespace ParlayRMS.Controllers
{
    public class ManageFunctionController : BaseController
    {
        private readonly FunctionsService _functionService = new FunctionsService();
        private readonly UsersService _userService = new UsersService();

        public ManageFunctionController()
        {
            ViewBag.HasWritePermission = _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageFunction);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageFunction, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            var model = _functionService.GetAllFunctionsInOrder();
            return View(model);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageFunction, Policy = UserPolicy.Create)]
        public ActionResult CreateFunction()
        {
            var allFunctions = _functionService.GetAllFunctions();
            var parentFunctions = allFunctions.Where(x => x.Parent == null);

            var functionSelectList = parentFunctions.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            functionSelectList.Insert(0, new SelectListItem { Value = "0", Text = ViewLang.None });
            ViewBag.FunctionSelectList = functionSelectList;

            var model = new CreateFunctionModel();
            return PartialView(model);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageFunction, Policy = UserPolicy.Create)]
        public ActionResult CreateFunction(CreateFunctionModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _functionService.CreateFunction(
                                                   model.Name,
                                                   model.Key,
                                                   model.Description,
                                                   model.ParentId,
                                                   model.ParentName,
                                                   model.DisplayOrder,
                                                   CurrentSession.UserId,
                                                   CurrentContext.Ip4Address,
                                                   out var functionId);
            return Json(new { code = result.Code, message = result.Message, functionId });
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageFunction, Policy = UserPolicy.Update)]
        public ActionResult EditFunction(int functionId)
        {
            var allFunctions = _functionService.GetAllFunctions();
            var parentFunctions = allFunctions.Where(x => x.Parent == null && x.Id != functionId);
            var function = allFunctions.Where(x => x.Id == functionId).FirstOrDefault();

            var functionSelectList = parentFunctions.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            functionSelectList.Insert(0, new SelectListItem { Value = "0", Text = ViewLang.None });
            ViewBag.FunctionSelectList = functionSelectList;

            var model = new EditFunctionModel
            {
                Id = function.Id,
                Name = function.Name,
                Description = function.Description,
                ParentId = function.Parent != null ? function.Parent.Id : 0,
                ParentName = function.Parent != null ? function.Parent.Name : string.Empty,
                DisplayOrder = function.DisplayOrder
            };

            return PartialView(model);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageFunction, Policy = UserPolicy.Update)]
        public ActionResult EditFunction(EditFunctionModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _functionService.UpdateFunction(
                                                   model.Id,
                                                   model.Name,
                                                   model.Description,
                                                   model.ParentId,
                                                   model.ParentName,
                                                   model.DisplayOrder,
                                                   CurrentSession.UserId,
                                                   CurrentContext.Ip4Address);
            return Json(result);
        }
    }
}
