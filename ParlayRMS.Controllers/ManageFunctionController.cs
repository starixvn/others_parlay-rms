namespace ParlayRMS.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using ParlayRMS.Services.Implement.Functions;
    using ParlayRMS.Services.Interface.Functions;
    using ParlayRMS.ViewModel.ManageFunction;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Utility;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Services.Implement.Users;

    public class ManageFunctionController : BaseController
    {
        private readonly IFunctionsService functionService = new FunctionsService();
        private readonly IUsersService userService = new UsersService();

        public ManageFunctionController()
        {
            ViewBag.HasWritePermission = userService.HasWritePermission(CurrentSession.UserId, (int)eFunction.ManageFunction);
        }

        // GET: ~/ManangeFunction
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.ManageFunction, Policy = ePolicy.Read)]
        public ActionResult Index()
        {
            var model = this.functionService.GetAllFunctionsInOrder();
            return this.View(model);
        }

        // GET: ~/ManangeFunction/CreateFunction
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.ManageFunction, Policy = ePolicy.Create)]
        public ActionResult CreateFunction()
        {
            var allFunctions = this.functionService.GetAllFunctions();
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

        // POST: ~/ManangeFunction/CreateFunction
        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.ManageFunction, Policy = ePolicy.Create)]
        public ActionResult CreateFunction(CreateFunctionModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            int functionId = 0;
            var result = this.functionService.CreateFunction(model.Name, model.Key, model.Description, model.ParentId, model.ParentName, model.DisplayOrder, CurrentSession.UserId, CurrentContext.Ip4Address, out functionId);
            return this.Json(new { code = result.Code, message = result.Message, functionId = functionId });
        }

        // GET: ~/ManangeFunction/EditFunction
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.ManageFunction, Policy = ePolicy.Update)]
        public ActionResult EditFunction(int functionId)
        {
            var allFunctions = this.functionService.GetAllFunctions();
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

        // GET: ~/ManangeFunction/EditFunction
        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.ManageFunction, Policy = ePolicy.Update)]
        public ActionResult EditFunction(EditFunctionModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.functionService.UpdateFunction(model.Id, model.Name, model.Description, model.ParentId, model.ParentName, model.DisplayOrder, CurrentSession.UserId, CurrentContext.Ip4Address);
            return this.Json(result);
        }
    }
}