using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    /// <summary>
    /// 共通的枚举类   
    /// </summary>
    public static class CommonEnums
    {
        /// <summary>
        /// 自动派工
        /// </summary>
        public static class AutomaticDispatch
        {
            /// <summary>
            /// 派工模式
            /// </summary>
            public enum Model
            {
                /// <summary>
                /// 完全自动
                /// </summary>
                COMPLETELY,
                /// <summary>
                /// 半自动
                /// </summary>
                ADJUSTMENT
            }

            /// <summary>
            /// 位置基准
            /// </summary>
            public enum BaseSection
            {
                /// <summary>
                /// 休息室
                /// </summary>
                LASTTASK,
                /// <summary>
                /// 上一次位置
                /// </summary>
                FOYER,
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public enum DispatchParamsCode
        {
            /// <summary>
            /// 
            /// </summary>
            PREMINUTES,
            /// <summary>
            /// 
            /// </summary>
            NEXTMINUTES,
        }


        /// <summary>
        /// 航班警告类型
        /// </summary>
        public enum FlightWarningType
        {
            /// <summary>
            /// 变更
            /// </summary>
            CHANGE,
            /// <summary>
            /// 新增
            /// </summary>
            NEW,
            /// <summary>
            /// 取消
            /// </summary>
            CNL
        }

        /// <summary>
        /// 航班状态
        /// </summary>
        public enum FlightStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            OP,
            /// <summary>
            /// 删除
            /// </summary>
            DEL,
            /// <summary>
            /// 取消
            /// </summary>
            CNL
        }

        /// <summary>
        /// 角色类型
        /// </summary>
        public enum Role
        {
            /// <summary>
            /// 系统管理员
            /// </summary>
            SYS,
            /// <summary>
            /// 航站基础数据管理员
            /// </summary>
            AIRPORT_DATA,
            /// <summary>
            /// 部门基础数据管理员
            /// </summary>
            DEPT_DATA,
            /// <summary>
            /// 调度部门基础数据
            /// </summary>
            DD_DATA,
            /// <summary>
            /// 部门调度员
            /// </summary>
            DEPT_DISPATCHER,
            /// <summary>
            /// 部门监控员
            /// </summary>
            DEPT_MONITOR,
            /// <summary>
            /// 部门排班管理员
            /// </summary>
            DEPT_SCHEDULER,
            /// <summary>
            /// 手持终端人员
            /// </summary>
            HAND_TERMINAL,
            /// <summary>
            /// 手持终端管理员
            /// </summary>
            HAND_TERMINALER_LEADER,
            /// <summary>
            /// 航站客户
            /// </summary>
            AIRPORT_GUEST,
            /// <summary>
            /// 部门客户
            /// </summary>
            DEPT_GUEST
        }

        /// <summary>
        /// 部门类型
        /// </summary>
        public enum Department
        {
            /// <summary>
            /// 普通部门
            /// </summary>
            COMMON,
            /// <summary>
            /// 调度部门
            /// </summary>
            DISPATCH,
            /// <summary>
            /// 监察部门
            /// </summary>
            SUPERVISE
        }

        /// <summary>
        /// CouchBaseKey
        /// </summary>
        public enum CouchBaseKey
        {
            /// <summary>
            /// +CommonEnums.Scope.PC.toString()
            /// </summary>
            USERIDS,
            /// <summary>
            /// +CommonEnums.Scope.PC.toString()
            /// </summary>
            ID_TOKEN,
            /// <summary>
            /// 
            /// </summary>
            LOGSTATUS,
            /// <summary>
            /// 
            /// </summary>
            GROUPRULE,
            /// <summary>
            /// 
            /// </summary>
            FLIGHTLEGTORULE,
            /// <summary>
            /// 
            /// </summary>
            MANUALFLIGHTRULEGROUP
        }

        /// <summary>
        /// 任务类型-普通任务，监控任务
        /// </summary>
        public enum TaskType
        {
            /// <summary>
            /// 普通任务
            /// </summary>
            COMMON,
            /// <summary>
            /// 监控任务
            /// </summary>
            MONITOR,
            /// <summary>
            /// 特殊任务
            /// </summary>
            SPECIAL
        }

        /// <summary>
        /// 任务生成方式-自动，手动
        /// </summary>
        public enum TaskCreateType
        {
            /// <summary>
            /// 自动
            /// </summary>
            AUTOMATIC,
            /// <summary>
            /// 手动
            /// </summary>
            MANUAL,
            /// <summary>
            /// 自动，手动
            /// </summary>
            ALL
        }

        /// <summary>
        /// 任务配置时间点类型-固定时常 窗口时常
        /// </summary>
        public enum TaskTimesType
        {
            /// <summary>
            /// 固定时常
            /// </summary>
            FIXED,
            /// <summary>
            /// 窗口时常
            /// </summary>
            WINDOW,
            /// <summary>
            /// 最小时常
            /// </summary>
            MIN
        }

        /// <summary>
        /// 告警处理状态-新建，已阅
        /// </summary>
        public enum WarnStatus
        {
            /// <summary>
            /// 新建
            /// </summary>
            NEW,
            /// <summary>
            /// 已阅
            /// </summary>
            READ
        }

        /// <summary>
        /// 任务状态-NEW 待接收 ACCEPT 接收 SUSPEND 挂起
        /// </summary>
        public enum TaskStatus
        {
            /// <summary>
            /// 新建
            /// </summary>
            NEW,
            /// <summary>
            /// 已分配
            /// </summary>
            DISTRIBUTE,
            /// <summary>
            /// 已接受
            /// </summary>
            ACCEPT,
            /// <summary>
            /// 开始
            /// </summary>
            BEGIN,
            /// <summary>
            /// 结束
            /// </summary>
            END,
            /// <summary>
            /// 停止
            /// </summary>
            STOP,
            /// <summary>
            /// 挂起
            /// </summary>
            SUSPEND,
            /// <summary>
            /// 取消
            /// </summary>
            CANCEL,
            /// <summary>
            /// 继续
            /// </summary>
            CONTINUE,
            /// <summary>
            /// 锁定
            /// </summary>
            LOCKED,
            /// <summary>
            /// 解除锁定
            /// </summary>
            UNLOCKED,

        }

        //    public enum TaskMatchType {
        //    SYS,//系统匹配模式
        //    MANUAL,//手动匹配模式
        //    }


        /// <summary>
        /// 时间点类型-开始 结束
        /// </summary>
        public enum TimesType
        {
            /// <summary>
            /// 开始
            /// </summary>
            BEGIN,
            /// <summary>
            /// 结束
            /// </summary>
            END
        }

        /// <summary>
        /// 运行时间等级-计划的 预计的 实际的
        /// </summary>
        public enum TimesClass
        {
            /// <summary>
            /// 计划的
            /// </summary>
            PLAN,
            /// <summary>
            /// 预计的
            /// </summary>
            EST,
            /// <summary>
            /// 实际的
            /// </summary>
            ACT
        }

        /// <summary>
        /// 运行符
        /// </summary>
        public enum MathOperation
        {
            /// <summary>
            /// 
            /// </summary>
            REGULAR,
            /// <summary>
            /// 
            /// </summary>
            EQUAL,
            /// <summary>
            /// 
            /// </summary>
            GREATER,
            /// <summary>
            /// 
            /// </summary>
            LESS
        }

        /// <summary>
        /// 是否标志
        /// </summary>
        public enum YesOrNo
        {
            /// <summary>
            /// 
            /// </summary>
            YES,
            /// <summary>
            /// 
            /// </summary>
            NO
        }

        /// <summary>
        /// 航班种类
        /// </summary>
        public enum InOrOut
        {
            /// <summary>
            /// 
            /// </summary>
            IN,
            /// <summary>
            /// 
            /// </summary>
            OUT,
            /// <summary>
            /// 
            /// </summary>
            INOUT
        }

        /// <summary>
        /// 性别
        /// </summary>
        public enum Gender
        {
            /// <summary>
            /// 
            /// </summary>
            MALE,
            /// <summary>
            /// 
            /// </summary>
            FEMALE
        }

        /// <summary>
        /// I国际, D国内
        /// </summary>
        public enum InterDome
        {
            /// <summary>
            /// I国际
            /// </summary>
            I,
            /// <summary>
            /// D国内
            /// </summary>
            D
        }

        /// <summary>
        /// 任务点击完成方式, PC端，Android端
        /// </summary>
        public enum CompleteType
        {
            PC, ANDROID
        }

        /// <summary>
        /// 权限类别
        /// </summary>
        public enum AuthorityType
        {
            ALL, WRITE, READ
        }

        /// <summary>
        /// 角色的作用范围 
        /// </summary>
        public enum RoleScope
        {
            /// <summary>
            /// 表示所有的航站/表示当前航站所有的部门
            /// </summary>
            ALL,
            /// <summary>
            /// 表示当前自己的航站/表示当前自己的部门
            /// </summary>
            SINGLE
        }

        /// <summary>
        /// 区域 PC和HAND_HOLD
        /// </summary>
        public enum Scope
        {
            PC, HAND_HOLD
        }

        /// <summary>
        /// 航班类型
        /// </summary>
        public enum FlightType
        {
            ARR, DEPT, CLUSTER
        }

        /// <summary>
        /// 距离规则
        /// </summary>
        public enum DistanceRule
        {
            /// <summary>
            /// 位置到其本身
            /// </summary>
            X2X,
            /// <summary>
            /// 相同位置下的任意两个位置
            /// </summary>
            X2Y,
            /// <summary>
            /// 子位置到父位置
            /// </summary>
            X2F,
            /// <summary>
            /// 其余位置默认距离
            /// </summary>
            X2O
        }

        /// <summary>
        /// 通过Code获取对应文本
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static string GetCode(this DistanceRule em)
        {
            string str = "";
            switch (em)
            {
                case DistanceRule.X2X:
                    str = "位置到其本身";
                    break;
                case DistanceRule.X2Y:
                    str = "相同位置下的任意两个位置";
                    break;
                case DistanceRule.X2F:
                    str = "子位置到父位置";
                    break;
                case DistanceRule.X2O:
                    str = "其余位置默认距离";
                    break;
                default:
                    break;
            }
            return str;
        }

        /// <summary>
        /// 获取Code的名称
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static string GetName(this DistanceRule em)
        {
            return em.ToString();
        }

        /// <summary>
        /// 返回状态
        /// </summary>
        public class ReturnStatus
        {
            /// <summary>
            /// 失败
            /// </summary>
            public static int FAIL = 0;

            /// <summary>
            /// 成功
            /// </summary>
            public static int SUCCESS = 1;

            /// <summary>
            /// 异常
            /// </summary>
            public static int EXCEPTION = 2;

            /// <summary>
            /// 退出
            /// </summary>
            public static int LOGOUT = 3;


        }

        /// <summary>
        /// 
        /// </summary>
        public class DutyType
        {
            /// <summary>
            /// 休息
            /// </summary>
            public static int REST = -1;
            /// <summary>
            /// 上班
            /// </summary>
            public static int WORKON = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public class flightWarningConfigType
        {
            /// <summary>
            /// 不捕获
            /// </summary>
            public static String NO_CATCH = "0";
            /// <summary>
            /// 存在捕获
            /// </summary>
            public static String EXIST_CATCH = "1";
            /// <summary>
            /// 变化捕获
            /// </summary>
            public static String CHANGE_CATCH = "2";
            /// <summary>
            /// 变化超过阈值捕获
            /// </summary>
            public static String THRESHOLD_CATCH = "3";
            /// <summary>
            /// 特殊捕获
            /// </summary>
            public static String SPECIAL_CATCH = "4";
        }

        /// <summary>
        /// 批量一次性提交数量
        /// </summary>
        public static int BatchNum = 1000;

        /// <summary>
        /// 任务数量超过N个的时候多开一个线程更新
        /// </summary>
        public static int UpdateNum = 25;

        /// <summary>
        /// 部门任务监控开始延误时间
        /// </summary>
        public static int DEAPRTMENTTASKDELAYBEGIN = 10;

        /// <summary>
        /// 部门任务监控结束延误时间
        /// </summary>
        public static int DEAPRTMENTTASKDELAYBEND = 10;

        /// <summary>
        /// 监察任务监控开始延误时间 
        /// </summary>
        public static int FIELDINSPECTIONDELAYBEGIN = 10;



    }
    public class PropertyItem
    {
        public PropertyItem(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name { set; get; }
        public Type Type { set; get; }
    }
    public class UserTypeFactory
    {
        public static Type GetUserType(params PropertyItem[] itemList)
        {
            TypeBuilder builder = CreateTypeBuilder(
                    "MyDynamicAssembly", "MyModule", "MyType");
            foreach (var item in itemList)
            {
                CreateAutoImplementedProperty(builder, item.Name, item.Type);
            }

            Type resultType = builder.CreateType();
            return resultType;
        }

        private static TypeBuilder CreateTypeBuilder(
            string assemblyName, string moduleName, string typeName)
        {
            TypeBuilder typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName),
                                       AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        private static void CreateAutoImplementedProperty(
            TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // 定义字段.
            FieldBuilder fieldBuilder = builder.DefineField(
                string.Concat(PrivateFieldPrefix, propertyName),
                              propertyType, FieldAttributes.Private);

            // 定义属性
            PropertyBuilder propertyBuilder = builder.DefineProperty(
                propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);

            // 属性的getter和setter的特性
            MethodAttributes propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig;

            // 定义getter方法
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(GetterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // 定义setter方法
            MethodBuilder setterMethod = builder.DefineMethod(
                string.Concat(SetterPrefix, propertyName),
                propertyMethodAttributes, null, new Type[] { propertyType });


            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }

    }
}
