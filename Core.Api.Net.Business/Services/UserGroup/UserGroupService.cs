using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.UserGroup;

namespace Core.Api.Net.Business.Services.UserGroup
{
    public class UserGroupService : IUserGroupService

    {
        private readonly ILogs _ILogs;
        private readonly IUserGroupRepository _IUserGroupRepository;
        public UserGroupService(ILogs ILogs, IUserGroupRepository IUserGroupRepository)
        {
            _ILogs = ILogs;
            _IUserGroupRepository = IUserGroupRepository;
        }
        public IList<UserGroupModel> GetListUserGroup()
        {
            IList<UserGroupModel> resp = new List<UserGroupModel>();
            try
            {
                resp = _IUserGroupRepository.GetListUserGroup();

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("User Group Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseUserGroup Validate_MasterUserGroup(UserGroupModel data)
        {
            ResponseUserGroup resp = new ResponseUserGroup();
            try
            {
                if (string.IsNullOrEmpty(data.code))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Code is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.name_thai))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Name Thai is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.name_eng))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Name English is required.";
                    return resp;
                }
                if (data.group_level <= 0)
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Level is required.";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Validate User Group Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseUserGroup Insert_UserGroup(UserGroupModel data)
        {
            ResponseUserGroup resp = new ResponseUserGroup();
            try
            {
                var _validate = Validate_MasterUserGroup(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _IUserGroupRepository.Insert_UserGroup(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Insert Successfull." : "Insert Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("User Group Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseUserGroup Update_UserGroup(UserGroupModel data)
        {
            ResponseUserGroup resp = new ResponseUserGroup();
            try
            {
                var _validate = Validate_MasterUserGroup(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _IUserGroupRepository.Update_UserGroup(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Update Successfull." : "Update Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("User Group Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseUserGroup Delete_UserGroup(UserGroupModel data)
        {
            ResponseUserGroup resp = new ResponseUserGroup();
            try
            {
                var _validate = Validate_MasterUserGroup(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _IUserGroupRepository.Delete_UserGroup(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Delete Successfull." : "Delete Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("User Group Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

    }
}