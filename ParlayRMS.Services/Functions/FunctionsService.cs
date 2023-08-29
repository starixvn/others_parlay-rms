namespace ParlayRMS.Services.Functions
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Functions;
    using Models.Functions;
    using DTO;

    public class FunctionsService : BaseService
    {
        private readonly FunctionsDataAccess _functionsDataAccess = new FunctionsDataAccess();

        public Result CreateFunction(
                          string name,
                          string key,
                          string description,
                          int parentId,
                          string parentName,
                          int displayOrder,
                          int adminId,
                          string ip,
                          out int functionId)
        {
            return _functionsDataAccess.RMS_FunctionIns(name, key, description, parentId, parentName, displayOrder, adminId, ip, out functionId);
        }

        public Result UpdateFunction(int id, string name, string description, int parentId, string parentName, int displayOrder, int adminId, string ip)
        {
            return _functionsDataAccess.RMS_FunctionUpd(id, name, description, parentId, parentName, displayOrder, adminId, ip);
        }

        public IEnumerable<Function> GetAllFunctions()
        {
            var data = _functionsDataAccess.RMS_FunctionSel();
            return data.Select(x => new Function
            {
                Id = x.FuncId,
                Name = x.FuncName,
                Key = x.FuncKey,
                Description = x.FuncDesc,
                Parent = x.ParentId == 0 ? null : new Function
                {
                    Id = x.ParentId,
                    Name = x.ParentName
                },
                DisplayOrder = x.DisplayOrder,
                ModifiedDate = x.ModifiedDate
            });
        }

        public IEnumerable<Function> GetAllFunctionsInOrder()
        {
            var functionList = GetAllFunctions();
            var parents = functionList.Where(x => x.Parent == null);
            var list = new List<Function>();
            foreach (var parent in parents)
            {
                list.Add(parent);
                var items = functionList.Where(x => x.Parent != null && x.Parent.Id == parent.Id);
                if (items.Any())
                {
                    list.AddRange(items);
                }
            }

            return list;
        }

        public IEnumerable<Function> GetRootFunctions()
        {
            var functionList = GetAllFunctions();
            var rootFunctionList = functionList.Where(x => x.Parent == null);
            return rootFunctionList;
        }
    }
}
