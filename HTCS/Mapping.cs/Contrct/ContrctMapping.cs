﻿using Model.Contrct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.cs.Contrct
{
    public  class ContrctMapping : BaseEntityTypeMap<T_Contrct>
    {
        protected override void IniMaps()
        {
            HasKey(m => m.Id);

            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("T_CONTRACT");
            Property(m => m.Id).HasColumnName("ID");
            Property(m => m.onlinesign).HasColumnName("ONLINESIGN");
            Property(m => m.BeginTime).HasColumnName("BEGINTIME");
            Property(m => m.EndTime).HasColumnName("ENDTIME");
            Property(m => m.Type).HasColumnName("TYPE");
            Property(m => m.PinLv).HasColumnName("PINLV");
            Property(m => m.FirstPay).HasColumnName("FIRSTPAY");
            Property(m => m.Recent).HasColumnName("RECENT");
            Property(m => m.PayType).HasColumnName("PAYTYPE");
            Property(m => m.DayRecnet).HasColumnName("DAYRECENT");
            Property(m => m.Deposit).HasColumnName("DEPOSIT");
            Property(m => m.Remark).HasColumnName("REMARK");
            Property(m => m.Enclosure).HasColumnName("ENCLOSURE");
            Property(m => m.CreateTime).HasColumnName("CREATETIME");
            Property(m => m.CreatePerson).HasColumnName("CREATEPERSON");
            Property(m => m.Status).HasColumnName("STATUS");
            Property(m => m.HouseId).HasColumnName("HOUSEID");
            Property(m => m.HouseType).HasColumnName("HOUSETYPE");
            Property(m => m.TeantId).HasColumnName("TREATID");
            Property(m => m.DepositType).HasColumnName("DEPOSITTYPE");
            Property(m => m.BeforeDay).HasColumnName("BEFOREDAY");
            Property(m => m.Recivetype).HasColumnName("RECIIVETYPE");

            Property(m => m.RecivedType).HasColumnName("RECIVEDTYPE");
            Property(m => m.RecivedAmount).HasColumnName("RECIVEDAMOUNT");
            Property(m => m.RecivedAccount).HasColumnName("RECIVEDACCOUNT");


            Property(m => m.adress).HasColumnName("ADRESS");
            Property(m => m.treatname).HasColumnName("TREATNAME");

            Property(m => m.CompanyId).HasColumnName("COMPANYID");

            Property(m => m.payee).HasColumnName("PAYEE");
            Property(m => m.accounts).HasColumnName("ACCOUNTS");
            Property(m => m.bank).HasColumnName("BANK");

            Property(m => m.outtime).HasColumnName("OUTTIME");

            Property(m => m.userid).HasColumnName("USERID");
            Property(m => m.eleccontract).HasColumnName("ELECCONTRACT");

            Property(m => m.isxuzu).HasColumnName("ISXUZU");
            Property(m => m.contracttype).HasColumnName("CONTRACTTYPE");
            Property(m => m.contractnum).HasColumnName("CONTRACTNUM");
        }
    }
}
