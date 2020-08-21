using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Services.Dashboard;
using Core.Api.Net.Business.Services.FuncMenu;
using Core.Api.Net.Business.Services.PermissionFunc;
using Core.Api.Net.Business.Services.PermissionGroup;
using Core.Api.Net.Business.Services.PermissionRole;
using Core.Api.Net.Business.Services.Register;
using Core.Api.Net.Business.Services.Role;
using Core.Api.Net.Business.Services.Section;
using Core.Api.Net.Business.Services.TokenKey;
using Core.Api.Net.Business.Services.User;
using Core.Api.Net.Business.Services.UserGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Net.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RbacController : ControllerBase
    {
        private readonly IDashboardService _IDashboardService;
        private readonly IRegisterService _IRegisterService;
        private readonly ITokenKeyService _ITokenKeyService;
        private readonly IUserGroupService _IUserGroupService;
        private readonly IUserService _IUserService;
        private readonly ISectionService _ISectionService;
        private readonly IRoleService _IRoleService;
        private readonly IFuncMenuService _IFuncMenuService;
        private readonly IPermissionGroupService _IPermissionGroupService;
        private readonly IPermissionRoleService _IPermissionRoleService;
        private readonly IPermissionFuncService _IPermissionFuncService;

        public RbacController(
            IDashboardService IDashboardService,
            IRegisterService IRegisterService,
            ITokenKeyService ITokenKeyService,
            IUserGroupService IUserGroupService,
            IUserService IUserService,
            ISectionService ISectionService,
            IRoleService IRoleService,
            IFuncMenuService IFuncMenuService,
            IPermissionGroupService IPermissionGroupService,
            IPermissionRoleService IPermissionRoleService,
            IPermissionFuncService IPermissionFuncService)
        {
            _IDashboardService = IDashboardService;
            _IRegisterService = IRegisterService;
            _ITokenKeyService = ITokenKeyService;
            _IUserGroupService = IUserGroupService;
            _IUserService = IUserService;
            _IRoleService = IRoleService;
            _ISectionService = ISectionService;
            _IFuncMenuService = IFuncMenuService;
            _IPermissionGroupService = IPermissionGroupService;
            _IPermissionRoleService = IPermissionRoleService;
            _IPermissionFuncService = IPermissionFuncService;
        }


        #region "Dashboard"

        [HttpGet("GetListUsageStatusLogs")]
        public ActionResult GetListUsageStatusLogs()
        {
            string _date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            var _data = _IDashboardService.GetUsageStatusLogs(_date);

            if (_data != null)
            {
                ResponseUsageStatus resp = new ResponseUsageStatus()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpGet("GetListStatisticRequestTokenLogs")]
        public ActionResult GetListStatisticRequestTokenLogs()
        {
            //string _date = "11/08/2020";
            string _date = DateTime.Now.ToString("dd/MM/yyyy");

            var _data = _IDashboardService.GetStatisticRequestTokenLogs(_date);

            if (_data != null)
            {
                ResponseRequestStatisticLog resp = new ResponseRequestStatisticLog()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpGet("GetListStatisticUsageLogs")]
        public ActionResult GetListStatisticUsageLogs()
        {
            //string _date = "11/08/2020";
            int _month = Convert.ToInt32(DateTime.Now.ToString("MM"));
            int _year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

            var _data = _IDashboardService.GetStatisticUsageLogs(_month, _year);

            if (_data != null)
            {
                ResponseStatisticSignInLog resp = new ResponseStatisticSignInLog()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpGet("GetListUsageOfCountry")]
        public ActionResult GetListUsageOfCountry()
        {
            var _data = _IDashboardService.GetUsageOfCountry();

            if (_data != null)
            {
                ResponseUsageOfCountry resp = new ResponseUsageOfCountry()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        #endregion

        #region "Registed"

        [HttpGet("GetListRegistered")]
        public ActionResult GetListRegister()
        {
            var _data = _IRegisterService.GetListRegister();

            if (_data != null)
            {
                ResponseRegister resp = new ResponseRegister()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("ActivateRegistered")]
        public ActionResult ActivateRegistered([FromBody] RegisterModel data)
        {
            ResponseRegister resp = _IRegisterService.ActivateRegistered(data);

            return Ok(new { resp });
        }

        [HttpPost("AddRegistered")]
        public ActionResult AddRegistered([FromBody] RegisterModel data)
        {
            ResponseRegister resp = _IRegisterService.Insert_Registered(data);

            return Ok(new { resp });
        }

        [HttpPost("UpdateRegistered")]
        public ActionResult UpdateRegistered([FromBody] RegisterModel data)
        {
            ResponseRegister resp = _IRegisterService.Update_Registered(data);

            return Ok(new { resp });
        }

        [HttpPost("DeleteRegistered")]
        public ActionResult DeleteRegistered([FromBody] RegisterModel data)
        {
            ResponseRegister resp = _IRegisterService.Delete_Registered(data);

            return Ok(new { resp });
        }

        #endregion

        #region "Token Key"

        [HttpGet("GetListTokenKey")]
        public ActionResult GetListTokenKey()
        {
            var _data = _ITokenKeyService.GetListTokenKey();

            if (_data != null)
            {
                ResponseTokenKey resp = new ResponseTokenKey()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("AddTokenKey")]
        public ActionResult AddTokenKey([FromBody] TokenKeyModel data)
        {
            ResponseTokenKey resp = _ITokenKeyService.Insert_TokenKey(data);

            return Ok(new { resp });
        }

        [HttpPost("UpdateTokenKey")]
        public ActionResult UpdateTokenKey([FromBody] TokenKeyModel data)
        {
            ResponseTokenKey resp = _ITokenKeyService.Update_TokenKey(data);

            return Ok(new { resp });
        }

        [HttpPost("DeleteTokenKey")]
        public ActionResult DeleteTokenKey([FromBody] TokenKeyModel data)
        {
            ResponseTokenKey resp = _ITokenKeyService.Delete_TokenKey(data);

            return Ok(new { resp });
        }

        #endregion

        #region "User Group"

        [HttpGet("GetListUserGroup")]
        public ActionResult GetListUserGroup()
        {
            var _data = _IUserGroupService.GetListUserGroup();

            if (_data != null)
            {
                ResponseUserGroup resp = new ResponseUserGroup()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("AddUserGroup")]
        public ActionResult AddUserGroup([FromBody] UserGroupModel data)
        {
            ResponseUserGroup resp = _IUserGroupService.Insert_UserGroup(data);

            return Ok(new { resp });
        }

        [HttpPost("UpdateUserGroup")]
        public ActionResult UpdateUserGroup([FromBody] UserGroupModel data)
        {
            ResponseUserGroup resp = _IUserGroupService.Update_UserGroup(data);

            return Ok(new { resp });
        }

        [HttpPost("DeleteUserGroup")]
        public ActionResult DeleteUserGroup([FromBody] UserGroupModel data)
        {
            ResponseUserGroup resp = _IUserGroupService.Delete_UserGroup(data);

            return Ok(new { resp });
        }

        #endregion

        #region "User"

        [HttpGet("GetListUser")]
        public ActionResult GetListUser()
        {
            var _data = _IUserService.GetListUser();

            if (_data != null)
            {
                ResponseUser resp = new ResponseUser()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("AddUser")]
        public ActionResult AddUser([FromBody] UserModel data)
        {
            ResponseUser resp = _IUserService.Insert_User(data);

            return Ok(new { resp });
        }

        [HttpPost("UpdateUser")]
        public ActionResult UpdateUser([FromBody] UserModel data)
        {
            ResponseUser resp = _IUserService.Update_User(data);

            return Ok(new { resp });
        }

        [HttpPost("DeleteUser")]
        public ActionResult DeleteUser([FromBody] UserModel data)
        {
            ResponseUser resp = _IUserService.Delete_User(data);

            return Ok(new { resp });
        }

        #endregion

        #region "Function"

        [HttpGet("GetListFunction")]
        public ActionResult GetListFunction()
        {
            var _data = _IFuncMenuService.GetListFunction();

            if (_data != null)
            {
                ResponseFunction resp = new ResponseFunction()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("AddFunction")]
        public ActionResult AddFunction([FromBody] FunctionModel data)
        {
            ResponseFunction resp = _IFuncMenuService.Insert_Function(data);

            return Ok(new { resp });
        }

        [HttpPost("UpdateFunction")]
        public ActionResult UpdateFunction([FromBody] FunctionModel data)
        {
            ResponseFunction resp = _IFuncMenuService.Update_Function(data);

            return Ok(new { resp });
        }

        [HttpPost("DeleteFunction")]
        public ActionResult DeleteFunction([FromBody] FunctionModel data)
        {
            ResponseFunction resp = _IFuncMenuService.Delete_Function(data);

            return Ok(new { resp });
        }

        #endregion

        #region "Permission Group"

        [HttpGet("GetListPermissionGroup/{GroupCode}")]
        public ActionResult GetListPermissionGroup(string GroupCode)
        {
            var _data = _IPermissionGroupService.GetListPermissionGroup(GroupCode);

            if (_data != null)
            {
                ResponsePermissionGroup resp = new ResponsePermissionGroup()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("UpdatePermissionGroup/{GroupCode}")]
        public ActionResult UpdatePermissionGroup(string GroupCode, [FromBody] IList<PermissionGroupModel> data)
        {
            ResponsePermissionGroup resp = _IPermissionGroupService.Update_PermissionGroup(GroupCode, data);

            return Ok(new { resp });
        }

        #endregion

        #region "Permission Role"

        [HttpGet("GetListPermissionRole/{GroupCode}")]
        public ActionResult GetListPermissionRole(string GroupCode)
        {
            var _data = _IPermissionRoleService.GetListPermissionRole(GroupCode);

            if (_data != null)
            {
                ResponsePermissionRole resp = new ResponsePermissionRole()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("UpdatePermissionRole/{GroupCode}")]
        public ActionResult UpdatePermissionRole(string GroupCode, [FromBody] IList<PermissionRoleModel> data)
        {
            ResponsePermissionRole resp = _IPermissionRoleService.Update_PermissionRole(GroupCode, data);

            return Ok(new { resp });
        }

        #endregion

        #region "Permission Function"

        [HttpGet("GetListPermissionFunc/{GroupCode}")]
        public ActionResult GetListPermissionFunc(string GroupCode)
        {
            var _data = _IPermissionFuncService.GetListPermissionFunc(GroupCode);

            if (_data != null)
            {
                ResponsePermissionFunc resp = new ResponsePermissionFunc()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("UpdatePermissionFunc/{GroupCode}")]
        public ActionResult UpdatePermissionFunc(string GroupCode, [FromBody] IList<PermissionFuncModel> data)
        {
            ResponsePermissionFunc resp = _IPermissionFuncService.Update_PermissionFunc(GroupCode, data);

            return Ok(new { resp });
        }

        #endregion

        #region "Section"

        [HttpGet("GetListSection")]
        public ActionResult GetListSection()
        {
            var _data = _ISectionService.GetListSection();

            if (_data != null)
            {
                ResponseSection resp = new ResponseSection()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("AddSection")]
        public ActionResult AddSection([FromBody] SectionModel data)
        {
            ResponseSection resp = _ISectionService.Insert_Section(data);

            return Ok(new { resp });
        }

        [HttpPost("UpdateSection")]
        public ActionResult UpdateSection([FromBody] SectionModel data)
        {
            ResponseSection resp = _ISectionService.Update_Section(data);

            return Ok(new { resp });
        }

        [HttpPost("DeleteSection")]
        public ActionResult DeleteSection([FromBody] SectionModel data)
        {
            ResponseSection resp = _ISectionService.Delete_Section(data);

            return Ok(new { resp });
        }

        #endregion

        #region "Role"

        [HttpGet("GetListRole")]
        public ActionResult GetListRole()
        {
            var _data = _IRoleService.GetListRole();

            if (_data != null)
            {
                ResponseRole resp = new ResponseRole()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("AddRole")]
        public ActionResult AddRole([FromBody] RoleModel data)
        {
            ResponseRole resp = _IRoleService.Insert_Role(data);

            return Ok(new { resp });
        }

        [HttpPost("UpdateRole")]
        public ActionResult UpdateRole([FromBody] RoleModel data)
        {
            ResponseRole resp = _IRoleService.Update_Role(data);

            return Ok(new { resp });
        }

        [HttpPost("DeleteRole")]
        public ActionResult DeleteRole([FromBody] RoleModel data)
        {
            ResponseRole resp = _IRoleService.Delete_Role(data);

            return Ok(new { resp });
        }

        #endregion


    }
}