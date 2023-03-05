using JX3CalculatorShared.Class;
using System;
using System.Collections.Generic;

namespace TLCalculator.Class
{
    public partial class SkillData : SkillDataBase
    {
        #region 成员

        public double IgnoreB_M { get; set; }
        public double IgnoreB_P { get; set; }
        public double M_APCoef { get; set; }
        public double P_APCoef => Info.P_AP_Coef;

        #endregion

        #region 构造

        /// <summary>
        /// 复制构造
        /// </summary>
        /// <param name="old"></param>
        public SkillData(SkillData old) : base(old)
        {
            Info = old.Info;
            IgnoreB_M = old.IgnoreB_M;
            IgnoreB_P = old.IgnoreB_P;
            M_APCoef = old.M_APCoef;
        }

        /// <summary>
        /// 重置此技能信息，删除所有秘籍效果
        /// </summary>
        public void Reset()
        {
            base.Reset();
            if (Info != null)
            {
                M_APCoef = Info.M_AP_Coef;
                IgnoreB_M = Info.IgnoreB_M;
                IgnoreB_P = Info.IgnoreB_P;
            }
        }


        /// <summary>
        /// 更新技能系数
        /// </summary>
        public void UpdateCoef()
        {
            var m_AP_Coef = GetAPCoef();
            M_APCoef = m_AP_Coef;
        }

        public override bool ApplyValueEffect(string key, double value)
        {
            var handled = base.ApplyValueEffect(key, value);
            if (!handled)
            {
                handled = true;
                switch (key)
                {
                    case "IgnoreB_P":
                        {
                            IgnoreB_P += value;
                            break;
                        }

                    case "IgnoreB_M":
                        {
                            IgnoreB_M += value;
                            break;
                        }

                    default:
                        {
                            handled = false;
                            throw new ArgumentException($"未识别的key！ {key}");
                        }
                }
            }

            return handled;
        }

        public override bool ApplyOtherEffect(string key, List<object> value)
        {
            var handled = base.ApplyOtherEffect(key, value);
            if (!handled)
            {
                handled = true;
                switch (key)
                {
                    case "ZMWS_Add_Dmg":
                        {
                            break;
                        }

                    default:
                        {
                            handled = false;
                            throw new ArgumentException($"未识别的key！ {key}");
                        }
                }
            }

            return handled;
        }

        #endregion
    }
}