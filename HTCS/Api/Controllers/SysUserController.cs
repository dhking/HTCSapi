using API.CommonControllers;
using ControllerHelper;
using DAL.Common;
using Microsoft.Owin;
using Model;
using Model.Base;
using Model.Bill;
using Model.Menu;
using Model.User;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
namespace Api.Controllers
{
    public class SysUserController : DataCenterController
    {
        SysUserService service = new SysUserService();

        //开通在线缴费
        [Route("api/Sysuser/Openpayment")]
        public SysResult Openpayment(T_SysUser model)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result = new SysResult();
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            user.yzm = model.yzm;
            return service.Openpay(user,1);
        }

        //关闭在线缴费
        [Route("api/Sysuser/closepayment")]
        public SysResult closepayment(T_SysUser model)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result = new SysResult();
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            user.yzm = model.yzm;
            return service.Openpay(user, 0);
        }
        

        [Route("api/Sysuser/Login")]
        public SysResult<T_SysUser> Login(T_SysUser model)
        {
            SysResult<T_SysUser> result = new SysResult<T_SysUser>();
            result = service.Login(model);
          
            return result;
        }
        [Route("api/Sysuser/Logout")]
        public SysResult Logout(T_SysUser model)
        {
            SysResult result = new SysResult();
            string token = GetSysToken();
            service.Logout(token);
            result = result.SuccessResult("退出成功");
            return result;
        }
        //查询房管员
        [Route("api/Sysuser/Queryfgy")]
        public SysResult<T_SysUser> Queryfgy(HouseModel model)
        {
            return service.Queryfgy(model);
        }
        //个人信息查询
        [Route("api/Sysuser/QueryUser")]
        public SysResult<T_SysUser> QueryUser(T_SysUser model)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            SysResult<T_SysUser> reslut = new SysResult<T_SysUser>();
            if (user == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            return service.QueryUser(model);
        }
        [Route("api/Sysuser/QueryUser1")]
        public SysResult<T_SysUser> QueryUser1(T_SysUser model)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            SysResult<T_SysUser> reslut = new SysResult<T_SysUser>();
            if (user == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            reslut.numberData = user;
            return reslut;
        }
        //个人信息查询APP
        [Route("api/Sysuser/appQueryUser")]
        public SysResult<T_SysUser> appQueryUser(T_SysUser model)
        {
            T_SysUser user=GetCurrentUser(GetSysToken());
            SysResult<T_SysUser> reslut = new SysResult<T_SysUser>();
            if (user == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            
            return service.QueryUser(user);
        }
        //修改个人资料头像和昵称
        [Route("api/User/updatedata")]
        public SysResult updatedata(T_SysUser req)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            SysResult reslut = new SysResult();
            if (user == null)
            {
                reslut.Code = 1002;
                reslut.Message = "请先登录";
                return reslut;
            }
            req.Id = user.Id;
            return service.updatedata(req);
        }
        //发送验证码
        [Route("api/SysUser/Sendyzm")]
        public SysResult Sendyzm(yzRequest req)
        {
            return service.Sendyzm(req);
        }
        //APP注册
        [Route("api/Sysuser/register")]
        public SysResult<T_SysUser> register(T_SysUser model)
        {
            model.userimg = "moren.png";
            model.nickname =model.Mobile;
            return service.register(model);
        }
        //申请提现
        [Route("api/Account/withdrawal")]
        public SysResult withdrawal(T_Record model)
        {
            SysResult result = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                return result = result.FailResult("用户不存在");
            }
            model.CompanyId = user.CompanyId;
            return service.withdrawal(model);
        }
       
        //APP找回密码
        [Route("api/Sysuser/Retrieve")]
        public SysResult<T_SysUser> Retrieve(T_SysUser model)
        {

            return service.Retrieve(model);
        }
        //新增用户
        [Route("api/Sysuser/addUser")]
        [JurisdictionAuthorize(name = new string[] { "zsysuser-edit-bt" })]
        
        public SysResult addUser(T_SysUser model)
        {
            model.userimg = "moren.png";
            model.nickname = model.Name;
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                SysResult result = new SysResult();
                result.Code = 1002;
                result.Message = "请先登录";
                return result;
            }
            model.CompanyId = user.CompanyId;
            return service.addUser(model);
        }
        
        [JurisdictionAuthorize(name = new string[] { "sysuser" })]
        [Route("api/Sysuser/Querylist")]
        public SysResult<List<T_SysUser>> Querylist(T_SysUser model)
        {
            SysResult<List<T_SysUser>> sysresult = new SysResult<List<T_SysUser>>();
            try
            {
                T_SysUser user = GetCurrentUser(GetSysToken());
                if (user == null)
                {
                    SysResult<List<T_SysUser>> result = new SysResult<List<T_SysUser>>();
                    result.Code = 1002;
                    result.Message = "请先登录";
                    return result;
                }
                model.CompanyId = user.CompanyId;
                InitPage(model.PageSize, (model.PageSize * model.PageIndex));
                sysresult = service.Querybase(model, this.OrderablePagination);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        [JurisdictionAuthorize(name = new string[] { "sysrole" })]
        [Route("api/Sysrole/Querylistrole")]
        public SysResult<List<T_SysRole>> Querylistrole(T_SysRole model)
        {
            SysResult<List<T_SysRole>> sysresult = new SysResult<List<T_SysRole>>();
            try
            {
                T_SysUser user = GetCurrentUser(GetSysToken());
                if (user == null)
                {
                    SysResult<List<T_SysRole>> result = new SysResult<List<T_SysRole>>();
                    result.Code = 1002;
                    result.Message = "请先登录";
                    return result;
                }
                model.CompanyId = user.CompanyId;
                InitPage(model.PageSize, (model.PageSize * model.PageIndex));
                sysresult = service.Querybaserole(model, this.OrderablePagination);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
      
        //查询分页按钮信息
        [Route("api/Sysrole/Querylistbutton")]
        public SysResult<List<T_Button>> Querylistbutton(T_Button model)
        {
            SysResult<List<T_Button>> sysresult = new SysResult<List<T_Button>>();
            try
            {
                InitPage(model.PageSize, (model.PageSize * model.PageIndex));
                sysresult = service.Querybasebutton(model, this.OrderablePagination);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        //查询所有角色信息
        [Route("api/Sysrole/Querylistrolenopage")]
        public SysResult<List<T_SysRole>> Querylistrolenopage(T_SysRole model)
        {
            SysResult<List<T_SysRole>> sysresult = new SysResult<List<T_SysRole>>();
            try
            {
                sysresult = service.Querylistrolenopage(model);
            }
            catch (Exception ex)
            {
                sysresult.Message = ex.ToString();
                sysresult.Code = 1;
            }
            return sysresult;
        }

        //查询所有店铺无分页
        [Route("api/Sysshop/Querylistshopnopage")]
        public SysResult<List<T_Shop>> Querylistshopnopage(T_Shop model)
        {
            SysResult<List<T_Shop>> sysresult = new SysResult<List<T_Shop>>();
            try
            {
                sysresult = service.Querylistshopnopage(model);
            }
            catch (Exception ex)
            {
                sysresult.Message = ex.ToString();
                sysresult.Code = 1;
            }
            return sysresult;
        }
        //编辑用户角色表
        [Route("api/Sysuserrole/edit")]
        public SysResult edit(PlAction<T_SysUserRole,T_SysUser> model)
        {
            SysResult sysresult = new SysResult();
            sysresult = service.Edit(model);
            return sysresult;
        }
        //保存按钮信息
        
        [Route("api/Sysuserbutton/edit")]
        public SysResult btedit(T_Button model)
        {
            SysResult sysresult = new SysResult();
            try
            {
                sysresult = service.btnEdit(model);
            }
            catch (Exception ex)
            {
                sysresult.Message = ex.ToString();
                sysresult.Code = 1;
            }
            return sysresult;
        }
        //删除按钮
        [HttpPost]
        [Route("api/Sysuserbutton/delete")]
        public SysResult delete(iids ids)
        {
            string[] model = ids.ids.Split(","[0]);
            SysResult result = new SysResult(0, "删除成功");
            foreach (var mo in model)
            {
                result = service.delete(long.Parse(mo));
            }
            return result;
        }
        //查询用户角色信息
        [Route("api/Sysrole/Querylistuserrole")]
        public SysResult<List<T_SysUserRole>> Querylistuserrole(T_SysUserRole model)
        {
            SysResult<List<T_SysUserRole>> sysresult = new SysResult<List<T_SysUserRole>>();
            try
            {
                if (model.edtype == 1)
                {
                    return sysresult;
                }
                InitPage(model.PageSize, (model.PageSize * model.PageIndex));
                sysresult = service.Querybaseuserrole(model, this.OrderablePagination);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        //查询用户角色信息不分页
        [Route("api/Sysrole/Querynopageuserrole")]
        public SysResult<List<WrapSysUserRole>> Querynopageuserrole(T_SysUserRole model)
        {
            SysResult<List<WrapSysUserRole>> sysresult = new SysResult<List<WrapSysUserRole>>();
            try
            {
                if (model.edtype == 1)
                {
                    return sysresult;
                }
                sysresult = service.Querynopageuserrole(model);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        //初始化加载权限信息
        [Route("api/Sysrole/Querypreetion")]
        public SysResult<List<Pression>> Querypreetion(Pression model)
        {
            SysResult<List<Pression>> sysresult = new SysResult<List<Pression>>();
            try
            {
                T_SysUser user = GetCurrentUser(GetSysToken());
                if (user == null)
                {
                    sysresult.Code = 1002;
                    sysresult.Message = "请先登录";
                    return sysresult;
                }
                
                sysresult = service.Querybasepression(model.RoleId,user.CompanyId);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        //根据用户id返回权限信息
        [Route("api/Sysrole/Querypreetionbyuser")]
        [HttpPost]
        public SysResult<List<Pression>> QueryPressionbuUser(T_SysUser model)
        {
            SysResult<List<Pression>> sysresult = new SysResult<List<Pression>>();
            try
            {

                sysresult = service.Querybasepressionbuuser(model.listrole.Select(p=>p.Id).ToList());
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
        //保存权限结构
        [Route("api/Sysrole/Savepreetion")]
        [HttpPost]
        public SysResult Savepreetion(WrapPression model)
        {
            SysResult sysresult = new SysResult();
            try
            {
                service.Savepression(model);
            }
            catch (Exception ex)
            {
                sysresult.Code = -1;
                sysresult.Message = ex.ToString();
            }
            return sysresult;
        }
   
    }
}