namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Implement;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.ViewModel.ManageFunction;

    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Web;
    using ParlayRMS.Services.Interface.Functions;
    using ParlayRMS.Services.Implement.Functions;

    public class ManageFunctionController : BaseController
    {
        private readonly IFunctionsService functionService = new FunctionsService();

        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.ManageFunction, Policy = ePolicy.Read)]
        public ActionResult Index()
        {
            var model = this.functionService.GetAllFunctionsInOrder();
            return this.View(model);
        }

        [AuthorizationAttribute(Function = eFunction.ManageFunction, Policy = ePolicy.Create)]
        public ActionResult CreateFunction()
        {
            ViewBag.FunctionList = this.functionService.GetRootFunctions();
            return this.PartialView();
        }

        [AuthorizationAttribute(Function = eFunction.ManageFunction, Policy = ePolicy.Create)]
        [ValidateInput(false)]
        public ActionResult SaveNewFunction(CreateFunctionModel model)
        {
            int functionId = 0;
            var result = this.functionService.CreateFunction(model.Name, model.Key, model.Description, model.ParentId, model.ParentName, model.DisplayOrder, CurrentSession.UserId, CurrentContext.Ip4Address, out functionId);
            return this.Json(new { code = result.Code, message = result.Message, functionId = functionId });
        }

        [AuthorizationAttribute(Function = eFunction.ManageFunction, Policy = ePolicy.Update)]
        public ActionResult EditFunction(int functionId)
        {
            var functionList = this.functionService.GetAllFunctions();
            var rootFunctionList = functionList.Where(x => x.Parent == null && x.Id != functionId);
            ViewBag.FunctionList = rootFunctionList;
            var function = functionList.Where(x => x.Id == functionId).FirstOrDefault();
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

        [AuthorizationAttribute(Function = eFunction.ManageFunction, Policy = ePolicy.Update)]
        public ActionResult SaveUpdateFunction(EditFunctionModel model)
        {
            var result = this.functionService.UpdateFunction(model.Id, model.Name, model.Description, model.ParentId, model.ParentName, model.DisplayOrder, CurrentSession.UserId, CurrentContext.Ip4Address);
            return this.Json(new { code = result.Code, message = result.Message });
        }

        public PartialViewResult NewBar()
        {
            return this.PartialView();
        }
    }
}