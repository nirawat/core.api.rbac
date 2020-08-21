using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.Section;

namespace Core.Api.Net.Business.Services.Section
{
    public class SectionService : ISectionService

    {
        private readonly ILogs _ILogs;
        private readonly ISectionRepository _ISectionRepository;
        public SectionService(ILogs ILogs, ISectionRepository ISectionRepository)
        {
            _ILogs = ILogs;
            _ISectionRepository = ISectionRepository;
        }

        public SectionModel GetSection(string code)
        {
            SectionModel resp = new SectionModel();
            try
            {
                resp = _ISectionRepository.GetSection(code);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Section Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<SectionModel> GetListSection()
        {
            IList<SectionModel> resp = new List<SectionModel>();
            try
            {
                resp = _ISectionRepository.GetListSection();

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Section Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseSection Validate_MasterSection(SectionModel data)
        {
            ResponseSection resp = new ResponseSection();
            try
            {
                if (string.IsNullOrEmpty(data.code))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Code is required.";
                    return resp;
                }
                if (data.code.Length < 3)
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Code is required 3 digit.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.name_thai))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Name (Thai) is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.name_eng))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Name (Eng) is required.";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Validate Section Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseSection Insert_Section(SectionModel data)
        {
            ResponseSection resp = new ResponseSection();
            try
            {
                var _validate = Validate_MasterSection(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var getSection = _ISectionRepository.GetSection(data.code);
                    if (getSection != null)
                    {
                        resp.status = StatusResponse.Error;
                        resp.message = "Section Code is duplicate.";
                        return resp;
                    }

                    var rest = _ISectionRepository.Insert_Section(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Insert Successfull." : "Insert Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Section Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseSection Update_Section(SectionModel data)
        {
            ResponseSection resp = new ResponseSection();
            try
            {
                var _validate = Validate_MasterSection(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var getSection = _ISectionRepository.GetSection(data.code);

                    if (getSection != null)
                    {
                        if (getSection.code != data.code)
                        {
                            resp.status = StatusResponse.Error;
                            resp.message = "Section Code is duplicate.";
                            return resp;
                        }
                    }

                    var rest = _ISectionRepository.Update_Section(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Update Successfull." : "Update Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Section Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseSection Delete_Section(SectionModel data)
        {
            ResponseSection resp = new ResponseSection();
            try
            {
                var _validate = Validate_MasterSection(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _ISectionRepository.Delete_Section(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Delete Successfull." : "Delete Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Section Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

    }
}