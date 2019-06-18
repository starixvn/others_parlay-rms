using System.Linq;
using System.Web.Mvc;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.Functions;
using ParlayRMS.Services.Implement.Users;
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

        /// <summary>
        /// Redirects to the Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageFunction, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            var model = this._functionService.GetAllFunctionsInOrder();
            return this.View(model);
        }

        /// <summary>
        /// GET method: redirects to the Create Function view.
        /// </summary>
        /// <returns>Create Function view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageFunction, Policy = UserPolicy.Create)]
        public ActionResult CreateFunction()
        {
            var allFunctions = this._functionService.GetAllFunctions();
            var parentFunctions = allFunctions.Where(x => x.Parent == null);

            var functionSelectList = parentFunctions.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            functionSelectList.Insert(0, new SelectListItem { Value = "0", Text = ViewLang.None });
            ViewBag.FunctionSelectList = functionSelectList;

            var model = new CreateFunctionModel();
            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: creates new function.
        /// </summary>
        /// <param name="model">The model contains the function information.</param>
        /// <returns>Whether the function is created successful or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageFunction, Policy = UserPolicy.Create)]
        public ActionResult CreateFunction(CreateFunctionModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            int functionId = 0;
            var result = this._functionService.CreateFunction(
                                                   model.Name,
                                                   model.Key,
                                                   model.Description,
                                                   model.ParentId,
                                                   model.ParentName,
                                                   model.DisplayOrder,
                                                   CurrentSession.UserId,
                                                   CurrentContext.Ip4Address,
                                                   out functionId);
            return this.Json(new { code = result.Code, message = result.Message, functionId = functionId });
        }

        /// <summary>
        /// GET method: redirects to the Edit Function view.
        /// </summary>
        /// <param name="functionId">The function identifier.</param>
        /// <returns>Edit Function view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageFunction, Policy = UserPolicy.Update)]
        public ActionResult EditFunction(int functionId)
        {
            var allFunctions = this._functionService.GetAllFunctions();
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

            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: edits the function.
        /// </summary>
        /// <param name="model">The model contains the function information.</param>
        /// <returns>Whether the function is edit successful or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageFunction, Policy = UserPolicy.Update)]
        public ActionResult EditFunction(EditFunctionModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._functionService.UpdateFunction(
                                                   model.Id,
                                                   model.Name,
                                                   model.Description,
                                                   model.ParentId,
                                                   model.ParentName,
                                                   model.DisplayOrder,
                                                   CurrentSession.UserId,
                                                   CurrentContext.Ip4Address);
            return this.Json(result);
        }
    }
}