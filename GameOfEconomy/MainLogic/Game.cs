using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfEconomy.MainLogic
{
    public class Game
    {
        public int CurrentStep;

        private DataGridWrapper gridWrapper;

        private List<EEndVar> endVars;
        public List<EInstrVar> instrVars;
        private List<EInterVar> interVars;
        private List<EConstVar> constVars;

        private Dictionary<string, int> variableIds;


        public Game(DataGridWrapper gridWrapper)
        {
            this.gridWrapper = gridWrapper;

            InitializeVariables();
            PassVariablesToGrid();

            SimulateZeroYear();
        }

        private void InitializeVariables()
        {
            endVars = new List<EEndVar>
            {
                new EEndVar { Name = "GDP.$.G", LongName = "Темп прироста реального ВНП (Валового Национального продукта)" },
                new EEndVar { Name = "GDP.$.IND", LongName = "Индекс реального ВНП" },
                new EEndVar { Name = "PCI.$.IND", LongName = "Индекс реального дохода на душу населения" },
                new EEndVar { Name = "BELT.TIGHT.IND", LongName = "Индекс поставок на внутренний рынок" },
                new EEndVar { Name = "PUBL.INFRA.ASSETS/GDP.Q", LongName = "Отношение общей инфраструктуры к ВНП" },
                new EEndVar { Name = "UNEMPL.Q", LongName = "Число безработных" },
                new EEndVar { Name = "EXT.DEBT.TOT.SER/EXP.Q", LongName = "Общие выплаты по внешнему долгу по отношению к экспорту" },
                new EEndVar { Name = "EXT.DEBT.PUBL/GDP.$.Q", LongName = "Отношение внешнего долга к ВНП" },
                new EEndVar { Name = "IMP.SURPL.$", LongName = "Реальный излишек импорта" },
                new EEndVar { Name = "INFLAT.R", LongName = "Уровень инфляции" },
                new EEndVar { Name = "BUDG.DEF.TOT/GNP.Q", LongName = "Общий дефицит бюджета по отношению к ВНП" },
                new EEndVar { Name = "BUDG.DEF.PRIM./GNP.Q", LongName = "Первичный дефицит бюджета по отношению к ВНП" },
                new EEndVar { Name = "NAT.DEBT/GNP.Q ", LongName = "Общественный долг по отношению к ВНП" },
                new EEndVar { Name = "NAT.DEBT.SER/GNP.Q", LongName = "Выплаты по общественному долгу по отношению к ВНП" },
                new EEndVar { Name = "GINI.Q", LongName = "Коэффициент Gini" },
                new EEndVar { Name = "UNEMPL.INC.Q", LongName = "Отношение дохода безработных к среднему доходу" },
                new EEndVar { Name = "UNEMPL.INC.MIN.Q", LongName = "Минимальное отношение дохода безработных к среднему доходу" },
                new EEndVar { Name = "OFFICER.INC.Q", LongName = "Отношение доходов служащих к среднему доходу" },
                new EEndVar { Name = "PENSIONERS.INC.Q", LongName = "Отношение дохода пенсионеров к среднему доходу" },
                new EEndVar { Name = "SOC.EXPEND.Q", LongName = "Отношение социальных расходов бюджета к общим расходам бюджета" },
                new EEndVar { Name = "SECUR.EXPEND.Q", LongName = "Отношение расходов бюджета на оборону к общим расходам бюджета" }
            };

            instrVars = new List<EInstrVar>
            {
                new EInstrVar { Name = "INCR.PRIV.Q", LongName = "Увеличение доли приватизированных предприятий" , Value = "0"},
                new EInstrVar { Name = "INCR.MARKET.Q", LongName = "Увеличение доли рыночной экономики"  , Value = "0"},
                new EInstrVar { Name = "IMPROV.INSTIT.Q", LongName = "Улучшение уровня образованности"  , Value = "0"},
                new EInstrVar { Name = "INCR.RESTRUCT.Q", LongName = "Увеличение уровня реорганизации"  , Value = "0"},
                new EInstrVar { Name = "BANKRUPTCY-Y-N", LongName = "Применение закона о банкротстве (да - нет)"  , Value = "0"},
                new EInstrVar { Name = "DEVAL.R", LongName = "Уровень девальвации"  , Value = "0"},
                new EInstrVar { Name = "CAP.CONTR.INTENS.Q", LongName = "Интенсивность управления выводом капитала"  , Value = "0.8"},
                new EInstrVar { Name = "INTEREST.NR", LongName = "Номинальная процентная ставка"  , Value = "0.03"},
                new EInstrVar { Name = "IMP.DUTY.R", LongName = "Уровень импортных пошлин"  , Value = "0.7"},
                new EInstrVar { Name = "TAX.TOT.R", LongName = "Общий уровень налогов"  , Value = "0.55"},
                new EInstrVar { Name = "PUBL.INV.G", LongName = "Темп прироста общественных вложений"  , Value = "0.06"},
                new EInstrVar { Name = "UNEMPL.INC.PC.G", LongName = "Темп прироста доходов безработных на душу населения"  , Value = "0.06"},
                new EInstrVar { Name = "OFFICER.INC.PC.G", LongName = "Темп прироста доходов служащих на душу населения"  , Value = "0.06"},
                new EInstrVar { Name = "PENSIONER.INC.PC.G", LongName = "Темп прироста доходов пенсионеров на душу населения"  , Value = "0.06"},
                new EInstrVar { Name = "SOC.EXPEND.G", LongName = "Темп прироста социальных расходов"  , Value = "0.06"},
                new EInstrVar { Name = "SECUR.EXPEND.G", LongName = "Темп прироста расходов на безопасность"  , Value = "0.06"}
            };

            interVars = new List<EInterVar>
            {
                new EInterVar { Name = "ABSORP.$", LongName = "Поглощение" },
                new EInterVar { Name = "ABSORP.$G", LongName = "Реальный прирост поглощения" },
                new EInterVar { Name = "GDP.$", LongName = "Реальный ВНП" },
                new EInterVar { Name = "GDP", LongName = "Номинальный ВНП" },
                new EInterVar { Name = "GNP.$", LongName = "Реальный ГНП" },
                new EInterVar { Name = "GNP.$G", LongName = "Реальный прирост ГНП" },
                new EInterVar { Name = "GNP.$.IND", LongName = "Индекс реального ГНП" },
                new EInterVar { Name = "GNP", LongName = "Номинальный ГНП" },
                new EInterVar { Name = "GNP.G", LongName = "Номинальный прирост ГНП" },
                new EInterVar { Name = "PCI", LongName = "Номинальный доход на душу населения" },
                new EInterVar { Name = "PCI.$", LongName = "Реальный доход на душу населения" },
                new EInterVar { Name = "PCI.$.G", LongName = "Темп прироста реального дохода на душу населения" },
                new EInterVar { Name = "PROD.NON.TRAD.$", LongName = "Реальная стоймость непродаваемой продукции" },
                new EInterVar { Name = "PROD.NON.TRAD.$.G", LongName = "Реальный прирост непродаваемой продукции" },
                new EInterVar { Name = "PROGR.R", LongName = "Темп развития" },
                new EInterVar { Name = "OFFICERS", LongName = "Число государственных служащих" },
                new EInterVar { Name = "OFFICERS.INC.PC", LongName = "Доход государственных служащих на душу населения" },
                new EInterVar { Name = "OFFICERS.INC.R.GR", LongName = "Номинальный рублевый прирост дохода государственных служащих" },
                new EInterVar { Name = "PENSIONERS", LongName = "Число пенсионеров" },
                new EInterVar { Name = "PENSIONER.INC.PC", LongName = "Доход пенсионеров на душу населения" },
                new EInterVar { Name = "PERS.WORK.AGE", LongName = "Число пенсионеров рабочего возраста" },
                new EInterVar { Name = "POPUL", LongName = "Численность населения" },
                new EInterVar { Name = "UNEMPLOYED", LongName = "Число неработающих" },
                new EInterVar { Name = "UNEMPL.INC.PC", LongName = "Доход на неработающего" },
                new EInterVar { Name = "CAP.MOV.PRIV.$", LongName = "Фактическое частное внешнее движение капитала" },
                new EInterVar { Name = "CAP.MOV.PREVENT.$", LongName = "Движение капитала защищенное государством" },
                new EInterVar { Name = "CAP.MOV.WANT.$", LongName = "Движение капитала требуемое частным сектором" },
                new EInterVar { Name = "CAP.MOV.PUBL.INDUCED.$", LongName = "Вынужденное движение капитала" },
                new EInterVar { Name = "CAP.MOV.PUBL.INDUCED/EXP.Q", LongName = "Отношение вынужденного движения капитала к экспорту" },
                new EInterVar { Name = "CAP.ACC.BAL.$", LongName = "Баланс основного счета" },
                new EInterVar { Name = "CUR.ACC.BAL.$", LongName = "Баланс текущего счета" },
                new EInterVar { Name = "DEVAL.EXPECT", LongName = "Ожидаемая девальвация" },
                new EInterVar { Name = "DEVAL.REAL", LongName = "Реальная девальвация" },
                new EInterVar { Name = "EXCHG.NR", LongName = "Номинальный обменный курс" },
                new EInterVar { Name = "EXT.INTEREST.R", LongName = "Уровень внешней привлекательности рынка" },
                new EInterVar { Name = "EXP.$.Q", LongName = "КОтношение экспорта к ВНП" },
                new EInterVar { Name = "EXP.$", LongName = "Реальный размер экспорта в $" },
                new EInterVar { Name = "EXP.$.G", LongName = "Реальное увеличение экспорта" },
                new EInterVar { Name = "EXP.NAT.PROD.$", LongName = "$ величина экспорта от национального продукта" },
                new EInterVar { Name = "EXP.NAT.PROD.$G", LongName = "Реальное увеличение экспорта от национального продукта" },
                new EInterVar { Name = "EXT.DEBT.PRIV.$", LongName = "Частный внешний долг в $" },
                new EInterVar { Name = "EXT.DEBT.PRIV.SER.$", LongName = "Обслуживание внешнего частного долга в $" },
                new EInterVar { Name = "EXT.DEBT.PUBL.SER.RU", LongName = "Обслуживание общего внешнего долга в руб" },
                new EInterVar { Name = "EXT.DEBT.PUBL.SER/EXPEND.TOT.Q", LongName = "Отношение платежей по внешнему долгу к общим расходам бюджета" },
                new EInterVar { Name = "EXT.DEBT.PUBLIC.SER.Q", LongName = "Обслуживание общего внешнего долга к ВНП" },
                new EInterVar { Name = "EXT.DEBT.PUBL.SER.$", LongName = "Обслуживание внешнего долга в $" },
                new EInterVar { Name = "EXT.PRIV.ASS.$", LongName = "Частные внешние фонды в $" },
                new EInterVar { Name = "EXT.DEBT.PUBL.$", LongName = "Общий внешний долг в $" },
                new EInterVar { Name = "EXT.DEBT.PUBL/GDP$.Q", LongName = "Общий внешний долг к ВНП" },
                new EInterVar { Name = "EXT.AID.$", LongName = "Внешняя помощь в $" },
                new EInterVar { Name = "EXT.AID.RU", LongName = "Внешняя помощь в руб" },
                new EInterVar { Name = "FDI.IN.$", LongName = "Иностранные прямые капиталовложения" },
                new EInterVar { Name = "FDI.IN./GDP.Q", LongName = "Отношение объема иностранных инвестиций к ВНП" },
                new EInterVar { Name = "IMP.$", LongName = "Импорт в $" },
                new EInterVar { Name = "IMP.$.Q", LongName = "Импорт к ВНП" },
                new EInterVar { Name = "IMP.FOR.NONTRAD.$", LongName = "Импорт для домашнего рынка в $" },
                new EInterVar { Name = "IMP.FOR.NONTRAD.$G", LongName = "Реальное увеличения импорта для домашнего рынка в $" },
                new EInterVar { Name = "IMP.FOR.EXP.$", LongName = "Импорт для производства экспортной продукции в $" },
                new EInterVar { Name = "INTEREST.PARITY", LongName = "Паритет по интересу" },
                new EInterVar { Name = "BUDG.DEF.TOT", LongName = "Дефицит бюджета в руб" },
                new EInterVar { Name = "BUDG.DEF.PRIM", LongName = "Первичный дефицит бюджета" },
                new EInterVar { Name = "BUDG.EXPEND.TOT", LongName = "Общие расходы бюджета" },
                new EInterVar { Name = "BUDG.EXPEND.TOT.G", LongName = "Темп роста номинальных расходов бюджета" },
                new EInterVar { Name = "BUDG.EXPEND.TOT/GNP.Q", LongName = "Расход бюджета к ВНП" },
                new EInterVar { Name = "BUDG.EXPEND.FREE", LongName = "Свободные бюджетные расходы" },
                new EInterVar { Name = "BUDG.EXPEND.FREE.G", LongName = "Темп роста свободных расходов бюджета" },
                new EInterVar { Name = "BUDG.EXPEND.CONTRACT", LongName = "Договорные бюджетные расходы" },
                new EInterVar { Name = "BUDG.REV.TOT", LongName = "Общие доходы бюджета" },
                new EInterVar { Name = "CAP.CONTR.ADMIN.EXPEND", LongName = "Расходы на управление движением капитала" },
                new EInterVar { Name = "CUSTOMS.REV", LongName = "Таможенные доходы" },
                new EInterVar { Name = "DEF.PUBL.ENTERPR", LongName = "Нехватка государственных предприятий" },
                new EInterVar { Name = "GOV.EXPEND.OFFICER", LongName = "Расходы на государственных служащих" },
                new EInterVar { Name = "GOV.EXPEND.OFFICER/EXPEND", LongName = "Отношение расходов на государственных служащих к расходу бюджета" },
                new EInterVar { Name = "IMPROV.INSTIT.EXPEND", LongName = "Расходы на улучшение образования" },
                new EInterVar { Name = "NAT.DEBT", LongName = "Национальный долг в руб" },
                new EInterVar { Name = "NAT.DEBT/GNP.Q", LongName = "Национальный долг к ВВП" },
                new EInterVar { Name = "NAT.DEBT.SER/BUDG.EXPEND.TOT.Q", LongName = "Обслуживание национального долга к общим расходам бюджета" },
                new EInterVar { Name = "NAT.DEBT.SER", LongName = "Обслуживание национального долга в руб" },
                new EInterVar { Name = "OFFICER.ADD.COST", LongName = "Дополнительные выплаты государственным служащим" },
                new EInterVar { Name = "OFFICER.INC.TOT", LongName = "Общий доход государственных служащих" },
                new EInterVar { Name = "PENSIONER.INC.TOT", LongName = "Общий доход пенсионеров" },
                new EInterVar { Name = "PENSIONER.INC.TOT/EXPEND.TOT.Q", LongName = "Увеличение выплат пенсионерам по отношению к общим расходам бюджета" },
                new EInterVar { Name = "PUBL.INFRA.ASSETS.$", LongName = "Размер общественной инфраструктуры в $" },
                new EInterVar { Name = "PUBL.INFRA.DEPREC.$", LongName = "Ежегодная переоценка общественной инфраструктуры" },
                new EInterVar { Name = "PUBL.INV", LongName = "Общественные вложения в руб" },
                new EInterVar { Name = "PUBL.INV/EXPEND.TOT.Q", LongName = "Отношение общественных вложений к расходам правительства" },
                new EInterVar { Name = "PUBL.INV.$", LongName = "Общественные вложения в $" },
                new EInterVar { Name = "PUBL.INV.$G", LongName = "Темп роста реальных общественных вложений" },
                new EInterVar { Name = "PUBL.INV.Q", LongName = "Общественные вложения в ВНП" },
                new EInterVar { Name = "RESTRUCT.EXPEND", LongName = "Расходы на реструктуризацию" },
                new EInterVar { Name = "SECUR.EXPEND", LongName = "Расходы на оборону" },
                new EInterVar { Name = "SECUR.EXPEND/EXPEND.Q", LongName = "Отношение расходов на безопасность к общим расходам правительства" },
                new EInterVar { Name = "SOC.EXPEND", LongName = "Расходы на социальную сферу" },
                new EInterVar { Name = "SOC.EXPEND/EXPEND.Q", LongName = "Отношение расходов на социальную сферу к общим расходам правительства" },
                new EInterVar { Name = "TAX.TOT.FACT.Q", LongName = "Фактический уровень налогов и социальные пожертвования" },
                new EInterVar { Name = "TAX.REV", LongName = "Налоговые поступления" },
                new EInterVar { Name = "UNEMPL.INC.TOT", LongName = "Общий доход безработных" },
                new EInterVar { Name = "UNEMPL.INC.TOT/EXPEND.TOT.G", LongName = "Увеличение платежей безработным к общим расходам правительства" },
                new EInterVar { Name = "EXCHG.VIRT.R", LongName = "Фактический валютный курс" },
                new EInterVar { Name = "PRICE.LEVEL.IND", LongName = "Индекс уровня цен" },
                new EInterVar { Name = "INTEREST.RR", LongName = "Реальный уровень привлекательности" },
                new EInterVar { Name = "STABILITY", LongName = "Показатель устойчивости рынка" },
                new EInterVar { Name = "CORP.Q", LongName = "Общий уровень рынка" },
                new EInterVar { Name = "CORRUPT.CALC", LongName = "Уровень коррупции" },
                new EInterVar { Name = "CORRUPT.FACTOR", LongName = "Показатель коррупционированности" },
                new EInterVar { Name = "MARKET.Q", LongName = "Уровень рыночной экономики" },
                new EInterVar { Name = "MARKET.INSTIT.Q", LongName = "Уровень рыночных институтов" },
                new EInterVar { Name = "PLAN.Q", LongName = "Уровень плановой экономики" },
                new EInterVar { Name = "PRIV.Q", LongName = "Уровень приватизации" },
                new EInterVar { Name = "RESTRUCT.Q", LongName = "Отношение рестроктурированных правительственных предприятий к общему числу предприятий" },
                new EInterVar { Name = "STRUCTURE", LongName = "Измеряет содействие структурных реформ прогрессированию фактора процесса" },
                new EInterVar { Name = "SYSTEM", LongName = "Измеряет содействие системных реформ прогрессированию фактора процесса" }
            };

            constVars = new List<EConstVar>
            {
                new EConstVar { Name = "debt.canc", LongName = "(Частично) аннулирование внешнего долга" },
                new EConstVar { Name = "ext.deb.ser.norm.q", LongName = "Норматив для коэффициента обслуживания внешнего долга" },
                new EConstVar { Name = "wm.g", LongName = "Темп роста мирового рынка" },
                new EConstVar { Name = "wm.interest.standard", LongName = "Стандартная международная процентная ставка" },
                new EConstVar { Name = "admin.cost-cap.control", LongName = "Воздействие контроля обращения капитала на административные расходы" },
                new EConstVar { Name = "prod-popul.g", LongName = "Воздействие роста численности населения на производство" },
                new EConstVar { Name = "prod-exp.loss", LongName = "Воздействие сокращения экспорта на производство" },
                new EConstVar { Name = "prod-marg.def", LongName = "Воздействие изменения бюджетного дефицита на производство" },
                new EConstVar { Name = "prod-customs", LongName = "Воздействие пошлин на импорт продукции" },
                new EConstVar { Name = "cap.exp-interest.parity", LongName = "Воздействие процентного паритета на экспорт капитала" },
                new EConstVar { Name = "deval.expect-deval.real0", LongName = "Воздействие реальной девальвации периода 0 на ожидание девальвации" },
                new EConstVar { Name = "deval.expect-deval.real-1", LongName = "Воздействие реальной девальвации периода -1 на ожидание девальвации" },
                new EConstVar { Name = "deval.expect-deval.real-2", LongName = "Воздействие реальной девальвации периода -2 на ожидание девальвации" },
                new EConstVar { Name = "eff.system-market-2", LongName = "Воздействие уровня рыночной экономики в периоде -2 на эффективность" },
                new EConstVar { Name = "eff.system-incr.market.-1", LongName = "Воздействие усиления уровня рыночной экономики в периоде -1 на эффективность" },
                new EConstVar { Name = "eff.system-incr.market.0", LongName = "Воздействие усиления системы рынка в периоде 0 на эффективность" },
                new EConstVar { Name = "eff.system-incr.market.1", LongName = "Воздействие усиления системы рынка в периоде 1 на эффективность" },
                new EConstVar { Name = "eff.system-plan", LongName = "Воздействие уровня планового хозяйства на эффективность" },
                new EConstVar { Name = "eff.system-priv", LongName = "Воздействие степени частного сектора на эффективность" },
                new EConstVar { Name = "eff.system-bankrupt", LongName = "Воздействие действующего закона о банкротстве на эффективность" },
                new EConstVar { Name = "eff.system-matket.instit", LongName = "Воздействие развитости рыночных институтов на эффективность" },
                new EConstVar { Name = "eff.struct-corrupt", LongName = "Воздействие коррупции на эффективность" },
                new EConstVar { Name = "eff.struct-public.infra", LongName = "Воздействие общественной инфраструктуры на эффективность" },
                new EConstVar { Name = "eff.struct-officers.y", LongName = "Воздействие уровня дохода должностного лица на эффективность" },
                new EConstVar { Name = "eff.struct-san", LongName = "Воздействие изменения структуры принадлежащих государству предприятий на эффективность" },
                new EConstVar { Name = "eff.struct-tax", LongName = "Воздействие налогового бремени на эффективность" },
                new EConstVar { Name = "eff.struct-customs", LongName = "Воздействие импортного таможенного тарифа на эффективность" },
                new EConstVar { Name = "eff.strukt-fdi", LongName = "Воздействие прямых инвестиций из-за границы на эффективность" },
                new EConstVar { Name = "eff.stabil-r.interest", LongName = "Воздействие реальной процентной ставки на эффективность" },
                new EConstVar { Name = "eff.stabil-infl>100", LongName = "Воздействие темпа инфляции большей 100% на эффективность" },
                new EConstVar { Name = "eff.stabil-infl>500", LongName = "Воздействие темпа инфляции большей 500% на эффективность" },
                new EConstVar { Name = "eff.stabil-infl>1000", LongName = "Воздействие темпа инфляции большей 1000% на эффективность" },
                new EConstVar { Name = "eff-neg.inflat", LongName = "Воздействие \"отрицательной\" инфляции (дефляция) на эффективность" },
                new EConstVar { Name = "gini-delta.corp", LongName = "Воздействие изменения уровня корпоратизованности на распределение дохода" },
                new EConstVar { Name = "gini-delta.instit", LongName = "Воздействие изменения ориентации институтов на рынок на распределение дохода" },
                new EConstVar { Name = "gini-delta.market", LongName = "Воздействие изменения уровня рынка на распределение дохода" },
                new EConstVar { Name = "gini-unemploy.inc", LongName = "Воздействие доходов безработного на распределение дохода" },
                new EConstVar { Name = "gini-pensioners.inc", LongName = "Воздействие дохода пенсионеров на распределение дохода" },
                new EConstVar { Name = "gini-soc.expend", LongName = "Воздействие расходов на социальные нужды на распределение дохода" },
                new EConstVar { Name = "imp-customs", LongName = "Воздействие тарифа на импорт" },
                new EConstVar { Name = "unempl.y-officers.inc", LongName = "Стандартный доход безработных, в сравнении с доходами должностного лица" },
                new EConstVar { Name = "unempl-bankrupt", LongName = "Воздействие действенности закона о банкротстве на безработицу" },
                new EConstVar { Name = "unempl-restruct", LongName = "Воздействие реструктуризации государственных предприятий на безработицу" },
                new EConstVar { Name = "unempl-priv", LongName = "Воздействие приватизации на безработицу" },
                new EConstVar { Name = "unempl-customs", LongName = "Воздействие пошлины на импорт на безработицу" },
                new EConstVar { Name = "employ-growth", LongName = "Воздействие роста экономики на занятость" },
                new EConstVar { Name = "exp-r.deval", LongName = "Воздействие реальной девальвации на экспорт" },
                new EConstVar { Name = "imp-r.deval", LongName = "Воздействие реальной девальвации на импорт" },
                new EConstVar { Name = "exp-wm.g", LongName = "Воздействие роста мирового рынка на экспорт" },
                new EConstVar { Name = "infl-r.deval", LongName = "Воздействие реальной девальвации на инфляцию" },
                new EConstVar { Name = "infl-marg.def", LongName = "Воздействие изменения бюджетного дефицита на инфляцию" },
                new EConstVar { Name = "infl-abs.def", LongName = "Воздействие уровня бюджетного дефицита на инфляцию" },
                new EConstVar { Name = "infl.abs-abs.def", LongName = "Воздействие бюджетного дефицита на инфляцию (при очень низких темпах инфляции)" },
                new EConstVar { Name = "infl-incr.market", LongName = "Воздействие изменения уровня системы рынка на инфляцию" },
                new EConstVar { Name = "infl-r.interest", LongName = "Воздействие реальной ставки на инфляцию" },
                new EConstVar { Name = "infl-tax.fact", LongName = "Воздействие фактической налоговой ставки на инфляцию" },
                new EConstVar { Name = "infl-customs", LongName = "Воздействие пошлины на импорт на инфляцию" },
                new EConstVar { Name = "corrupt-officer.inc", LongName = "Воздействие доходов должностного лица на коррупцию" },
                new EConstVar { Name = "corrupt-instit", LongName = "Воздействие ориентированности государственных учреждений на коррупцию" },
                new EConstVar { Name = "corrupt-security", LongName = "Воздействие расходов на безопасность на коррупцию" },
                new EConstVar { Name = "restruct.cost-def", LongName = "Воздействие дефицита госпредприятий на цену реструктуризации" },
                new EConstVar { Name = "tax.fact-officer.inc", LongName = "Воздействие доходов должностного лица на фактические налоговые сборы" },
                new EConstVar { Name = "tax.fact-tax.legal", LongName = "Воздействие законного уровня налогов на фактические налоговые сборы" },
                new EConstVar { Name = "tax.fact-market", LongName = "Воздействие усиления системы рынка на фактические налоговые сборы" },
                new EConstVar { Name = "tax.fact-instit", LongName = "Воздействие ориентированности государственных учреждений на рынок на фактические налоговые сборы" },
                new EConstVar { Name = "exp-time", LongName = "Воздействие времени на экспорт" },
                new EConstVar { Name = "unempl.death.q", LongName = "Смертность безработных" },
                new EConstVar { Name = "officer.q", LongName = "Процент должностных лиц в трудоспособном возрасте" },
                new EConstVar { Name = "officer.sidecost.q", LongName = "Дополнительные расходы на взятие в штат должностного лица в доходе должностного лица" },
                new EConstVar { Name = "officer.inc.norm.q", LongName = "Стандартный доход должностного лица, в % среднего дохода населения" },
                new EConstVar { Name = "popul.g", LongName = "Темп роста населения" },
                new EConstVar { Name = "publ.infra.deprec", LongName = "Ежегодная девальвация общественной части капитала, воплощенной в средствах труда" },
                new EConstVar { Name = "comecon.breakoff", LongName = "Крушение СЭВ" },
                new EConstVar { Name = "workers.q", LongName = "Процент трудоспособного населения во всем населении" },
                new EConstVar { Name = "aid.rate", LongName = "Иностранная помощь, в % национального дохода" },
                new EConstVar { Name = "aid.level", LongName = "Уровень дохода на душу населения; при потере внешних вложений" },
                new EConstVar { Name = "non.acc.empl.gr", LongName = "Уровень роста экономики; при спаде трудоустройства безработных" },
                new EConstVar { Name = "max.a.gr.int", LongName = "Процентная ставка, которая наиболее способствует экономическому росту" },
                new EConstVar { Name = "non.a.gr.int", LongName = "Процентная ставка препятствующая экономическому росту" },
                new EConstVar { Name = "non.dest.gr.infl", LongName = "Темп инфляции который не ущемляет рост экономики" },
                new EConstVar { Name = "non.dest.absorp.g.exp.g", LongName = "Темп роста экспорта, который не ущемляет производство для внутреннего рынка (поглощение)" },
                new EConstVar { Name = "reform.level", LongName = "Минимальный уровень экономических реформ для получения внешней помощи" },
                new EConstVar { Name = "pensioner.q", LongName = "Процент пенсионеров во всем населении" },
                new EConstVar { Name = "publ.inv.norm.q", LongName = "Стандартный уровень для общественных капиталовложений в инфраструктуру" },
                new EConstVar { Name = "secur.norm.q", LongName = "Стандартный уровень для расходов на безопасность" },
                new EConstVar { Name = "tax.soc.opt.q", LongName = "Уровень налогов и социальных сборов оптимальный для экономического роста" },
                new EConstVar { Name = "retrain.cost.q", LongName = "Цена переобучения служащих" },
                new EConstVar { Name = "wm.interest-ext.deb.ser.q", LongName = "Воздействие уровня расходов на обслуживание долга на внешнюю процентную ставку" }
            };
        }

        private void PassVariablesToGrid()
        {
            int counter = 0;

            variableIds = new Dictionary<string, int>();

            foreach (EVariable elem in endVars)
            {
                gridWrapper.AddRow(new DataGridRow
                {
                    Name = elem.Name,
                    LongName = elem.LongName
                });
                variableIds.Add(elem.Name, counter++);
            }

            foreach (EVariable elem in instrVars)
            {
                gridWrapper.AddRow(new DataGridRow
                {
                    Name = elem.Name,
                    LongName = elem.LongName
                });
                variableIds.Add(elem.Name, counter++);
            }

            foreach (EVariable elem in interVars)
            {
                gridWrapper.AddRow(new DataGridRow
                {
                    Name = elem.Name,
                    LongName = elem.LongName
                });
                variableIds.Add(elem.Name, counter++);
            }

            foreach (EVariable elem in constVars)
            {
                gridWrapper.AddRow(new DataGridRow
                {
                    Name = elem.Name,
                    LongName = elem.LongName
                });
                variableIds.Add(elem.Name, counter++);
            }

            gridWrapper.Apply();
        }

        private void SimulateZeroYear()
        {
            SetVar("GDP.$.G", 0.015f);
            SetVar("GDP.$.IND", 100.0f);
            SetVar("PCI.$.IND", 100.0f);
            SetVar("BELT.TIGHT.IND", 100.0f);
            SetVar("PUBL.INFRA.ASSETS/GDP.Q", 1.0f);
            SetVar("UNEMPL.Q", 0.001f);
            SetVar("EXT.DEBT.TOT.SER/EXP.Q", 0.31579999999999997f);
            SetVar("EXT.DEBT.PUBL/GDP.$.Q", 0.8333f);
            SetVar("IMP.SURPL.$", 10000000000.0f);
            SetVar("INFLAT.R", 0.03f);
            SetVar("BUDG.DEF.TOT/GNP.Q", -0.0342f);
            SetVar("BUDG.DEF.PRIM./GNP.Q", -0.1183f);
            SetVar("NAT.DEBT/GNP.Q", 0.7375f);
            SetVar("NAT.DEBT/GNP.Q ", 0.7375f);
            SetVar("NAT.DEBT.SER/GNP.Q", 0.0221f);
            SetVar("GINI.Q", 0.25f);
            SetVar("UNEMPL.INC.Q", 1.5487000000000002f);
            SetVar("UNEMPL.INC.MIN.Q", 1.5f);
            SetVar("OFFICER.INC.Q", 1.9912f);
            SetVar("PENSIONERS.INC.Q", 1.1062f);
            SetVar("SOC.EXPEND.Q", 0.1106f);
            SetVar("SECUR.EXPEND.Q", 0.13269999999999998f);
            SetVar("INCR.PRIV.Q", 0.0f);
            SetVar("INCR.MARKET.Q", 0.0f);
            SetVar("IMPROV.INSTIT.Q", 0.0f);
            SetVar("INCR.RESTRUCT.Q", 0.0f);
            SetVar("BANKRUPTCY-Y-N", 0.0f);
            SetVar("DEVAL.R", 0.0f);
            SetVar("CAP.CONTR.INTENS.Q", 0.8f);
            SetVar("INTEREST.NR", 0.03f);
            SetVar("IMP.DUTY.R", 0.7f);
            SetVar("TAX.TOT.R", 0.55f);
            SetVar("PUBL.INV.G", 0.06f);
            SetVar("UNEMPL.INC.PC.G", 0.06f);
            SetVar("OFFICER.INC.PC.G", 0.06f);
            SetVar("PENSIONER.INC.PC.G", 0.06f);
            SetVar("SOC.EXPEND.G", 0.06f);
            SetVar("SECUR.EXPEND.G", 0.06f);
            SetVar("ABSORP.$", 730000000000.0f);
            SetVar("ABSORP.$G", 0.0f);
            SetVar("GDP.$", 720000000000.0f);
            SetVar("GDP", 720000000000.0f);
            SetVar("GNP.$", 680000000000.0f);
            SetVar("GNP.$G", 0.015f);
            SetVar("GNP.$.IND", 100.0f);
            SetVar("GNP", 680000000000.0f);
            SetVar("GNP.G", 0.045f);
            SetVar("PCI", 4500.0f);
            SetVar("PCI.$", 4500.0f);
            SetVar("PCI.$.G", -0.01f);
            SetVar("PROD.NON.TRAD.$", 600000000000.0f);
            SetVar("PROD.NON.TRAD.$.G", 0.06f);
            SetVar("PROGR.R", 0.0f);
            SetVar("OFFICERS", 3000000.0f);
            SetVar("OFFICERS.INC.PC", 9000.0f);
            SetVar("OFFICERS.INC.R.GR", 0.01f);
            SetVar("PENSIONERS", 27000000.0f);
            SetVar("PENSIONER.INC.PC", 5000.0f);
            SetVar("PERS.WORK.AGE", 60000000.0f);
            SetVar("POPUL", 150000000.0f);
            SetVar("UNEMPLOYED", 60000.0f);
            SetVar("UNEMPL.INC.PC", 7000.0f);
            SetVar("CAP.MOV.PRIV.$", -27000000.0f);
            SetVar("CAP.MOV.PREVENT.$", -110000000.0f);
            SetVar("CAP.MOV.WANT.$", -130000000.0f);
            SetVar("CAP.MOV.PUBL.INDUCED.$", 52000000000.0f);
            SetVar("CAP.MOV.PUBL.INDUCED/EXP.Q", 0.3912f);
            SetVar("CAP.ACC.BAL.$", 52000000000.0f);
            SetVar("CUR.ACC.BAL.$", -52000000000.0f);
            SetVar("DEVAL.EXPECT", 0.0f);
            SetVar("DEVAL.REAL", 0.0f);
            SetVar("EXCHG.NR", 1.0f);
            SetVar("EXT.INTEREST.R", 0.07f);
            SetVar("EXP.$.Q", 0.1847f);
            SetVar("EXP.$", 130000000000.0f);
            SetVar("EXP.$.G", 0.01f);
            SetVar("EXP.NAT.PROD.$", 120000000000.0f);
            SetVar("EXP.NAT.PROD.$G", 0.0f);
            SetVar("EXT.DEBT.PRIV.$", 0.0f);
            SetVar("EXT.DEBT.PRIV.SER.$", 0.0f);
            SetVar("EXT.DEBT.PUBL.SER.RU", 42000000000.0f);
            SetVar("EXT.DEBT.PUBL.SER/EXPEND.TOT.Q", 0.0888f);
            SetVar("EXT.DEBT.PUBLIC.SER.Q", 0.058300000000000005f);
            SetVar("EXT.DEBT.PUBL.SER.$", 42000000000.0f);
            SetVar("EXT.PRIV.ASS.$", 1000000000.0f);
            SetVar("EXT.DEBT.PUBL.$", 600000000000.0f);
            SetVar("EXT.DEBT.PUBL/GDP$.Q", 0.8333f);
            SetVar("EXT.AID.$", 0.0f);
            SetVar("EXT.AID.RU", 0.0f);
            SetVar("FDI.IN.$", 0.0f);
            SetVar("FDI.IN./GDP.Q", 0.0f);
            SetVar("IMP.$", 140000000000.0f);
            SetVar("IMP.$.Q", 0.1986f);
            SetVar("IMP.FOR.NONTRAD.$", 130000000000.0f);
            SetVar("IMP.FOR.NONTRAD.$G", 0.0f);
            SetVar("IMP.FOR.EXP.$", 13000000000.0f);
            SetVar("INTEREST.PARITY", 0.01f);
            SetVar("BUDG.DEF.TOT", -23000000000.0f);
            SetVar("BUDG.DEF.PRIM", -80000000000.0f);
            SetVar("BUDG.EXPEND.TOT", 470000000000.0f);
            SetVar("BUDG.EXPEND.TOT.G", 0.0f);
            SetVar("BUDG.EXPEND.TOT/GNP.Q", 0.6975f);
            SetVar("BUDG.EXPEND.FREE", 370000000000.0f);
            SetVar("BUDG.EXPEND.FREE.G", 0.0f);
            SetVar("BUDG.EXPEND.CONTRACT", 110000000000.0f);
            SetVar("BUDG.REV.TOT", 500000000000.0f);
            SetVar("CAP.CONTR.ADMIN.EXPEND", 0.0f);
            SetVar("CUSTOMS.REV", 100000000000.0f);
            SetVar("DEF.PUBL.ENTERPR", 50000000000.0f);
            SetVar("GOV.EXPEND.OFFICER", 41000000000.0f);
            SetVar("GOV.EXPEND.OFFICER/EXPEND", 0.08560000000000001f);
            SetVar("IMPROV.INSTIT.EXPEND", 0.0f);
            SetVar("NAT.DEBT", 500000000000.0f);
            SetVar("NAT.DEBT/GNP.Q", 0.7375f);
            SetVar("NAT.DEBT.SER/BUDG.EXPEND.TOT.Q", 0.0317f);
            SetVar("NAT.DEBT.SER", 15000000000.0f);
            SetVar("OFFICER.ADD.COST", 14000000000.0f);
            SetVar("OFFICER.INC.TOT", 27000000000.0f);
            SetVar("PENSIONER.INC.TOT", 140000000000.0f);
            SetVar("PENSIONER.INC.TOT/EXPEND.TOT.Q", 0.28550000000000003f);
            SetVar("PUBL.INFRA.ASSETS.$", 720000000000.0f);
            SetVar("PUBL.INFRA.DEPREC.$", 13000000000.0f);
            SetVar("PUBL.INV", 25000000000.0f);
            SetVar("PUBL.INV/EXPEND.TOT.Q", 0.0529f);
            SetVar("PUBL.INV.$", 25000000000.0f);
            SetVar("PUBL.INV.$G", 0.02f);
            SetVar("PUBL.INV.Q", 0.0369f);
            SetVar("RESTRUCT.EXPEND", 0.0f);
            SetVar("SECUR.EXPEND", 90000000000.0f);
            SetVar("SECUR.EXPEND/EXPEND.Q", 0.19030000000000002f);
            SetVar("SOC.EXPEND", 75000000000.0f);
            SetVar("SOC.EXPEND/EXPEND.Q", 0.1586f);
            SetVar("TAX.TOT.FACT.Q", 0.55f);
            SetVar("TAX.REV", 400000000000.0f);
            SetVar("UNEMPL.INC.TOT", 420000000.0f);
            SetVar("UNEMPL.INC.TOT/EXPEND.TOT.G", 0.0009f);
            SetVar("EXCHG.VIRT.R", 1.0f);
            SetVar("PRICE.LEVEL.IND", 100.0f);
            SetVar("INTEREST.RR", 0.05f);
            SetVar("STABILITY", 0.0f);
            SetVar("CORP.Q", 0.0f);
            SetVar("CORRUPT.CALC", 0.0f);
            SetVar("CORRUPT.FACTOR", 0.0f);
            SetVar("MARKET.Q", 0.1f);
            SetVar("MARKET.INSTIT.Q", 0.1f);
            SetVar("PLAN.Q", 0.9f);
            SetVar("PRIV.Q", 0.1f);
            SetVar("RESTRUCT.Q", 0.1f);
            SetVar("STRUCTURE", 0.0f);
            SetVar("SYSTEM", 0.0f);
            SetVar("debt.canc", 0.0f);
            SetVar("ext.deb.ser.norm.q", 0.3f);
            SetVar("wm.g", 0.04f);
            SetVar("wm.interest.standard", 0.06f);
            SetVar("admin.cost-cap.control", 0.1f);
            SetVar("prod-popul.g", 0.5f);
            SetVar("prod-exp.loss", 2.0f);
            SetVar("prod-marg.def", 0.05f);
            SetVar("prod-customs", 0.3f);
            SetVar("cap.exp-interest.parity", 0.1f);
            SetVar("deval.expect-deval.real0", 0.2f);
            SetVar("deval.expect-deval.real-1", 0.1f);
            SetVar("deval.expect-deval.real-2", 0.05f);
            SetVar("eff.system-market-2", 0.02f);
            SetVar("eff.system-incr.market.-1", 0.0f);
            SetVar("eff.system-incr.market.0", -0.02f);
            SetVar("eff.system-incr.market.1", -0.01f);
            SetVar("eff.system-plan", 0.01f);
            SetVar("eff.system-priv", 0.01f);
            SetVar("eff.system-bankrupt", 0.01f);
            SetVar("eff.system-matket.instit", 0.01f);
            SetVar("eff.struct-corrupt", 0.01f);
            SetVar("eff.struct-public.infra", 0.07f);
            SetVar("eff.struct-officers.y", 0.03f);
            SetVar("eff.struct-san", 0.02f);
            SetVar("eff.struct-tax", 0.01f);
            SetVar("eff.struct-customs", 0.05f);
            SetVar("eff.strukt-fdi", 0.3f);
            SetVar("eff.stabil-r.interest", 0.1f);
            SetVar("eff.stabil-infl>100", 0.02f);
            SetVar("eff.stabil-infl>500", 0.03f);
            SetVar("eff.stabil-infl>1000", 0.04f);
            SetVar("eff-neg.inflat", 0.1f);
            SetVar("gini-delta.corp", 0.2f);
            SetVar("gini-delta.instit", 0.2f);
            SetVar("gini-delta.market", 0.3f);
            SetVar("gini-unemploy.inc", 0.1f);
            SetVar("gini-pensioners.inc", 0.1f);
            SetVar("gini-soc.expend", 0.5f);
            SetVar("imp-customs", 0.5f);
            SetVar("unempl.y-officers.inc", 0.7f);
            SetVar("unempl-bankrupt", 0.01f);
            SetVar("unempl-restruct", 0.1f);
            SetVar("unempl-priv", 0.3f);
            SetVar("unempl-customs", 0.5f);
            SetVar("employ-growth", 1.0f);
            SetVar("exp-r.deval", 0.5f);
            SetVar("imp-r.deval", 0.5f);
            SetVar("exp-wm.g", 1.0f);
            SetVar("infl-r.deval", 1.0f);
            SetVar("infl-marg.def", 1.3f);
            SetVar("infl-abs.def", 3.0f);
            SetVar("infl.abs-abs.def", 1.0f);
            SetVar("infl-incr.market", 2.0f);
            SetVar("infl-r.interest", 1.5f);
            SetVar("infl-tax.fact", 1.0f);
            SetVar("infl-customs", 1.0f);
            SetVar("corrupt-officer.inc", 1.0f);
            SetVar("corrupt-instit", 1.0f);
            SetVar("corrupt-security", 1.0f);
            SetVar("restruct.cost-def", 5.0f);
            SetVar("tax.fact-officer.inc", 0.05f);
            SetVar("tax.fact-tax.legal", 0.8f);
            SetVar("tax.fact-market", 0.5f);
            SetVar("tax.fact-instit", 0.5f);
            SetVar("exp-time", 0.04f);
            SetVar("unempl.death.q", 0.04f);
            SetVar("officer.q", 0.05f);
            SetVar("officer.sidecost.q", 0.5f);
            SetVar("officer.inc.norm.q", 2.5f);
            SetVar("popul.g", 0.0f);
            SetVar("publ.infra.deprec", 0.02f);
            SetVar("comecon.breakoff", 0.0f);
            SetVar("workers.q", 0.4f);
            SetVar("aid.rate", 0.3f);
            SetVar("aid.level", 80.0f);
            SetVar("non.acc.empl.gr", 0.02f);
            SetVar("max.a.gr.int", 0.05f);
            SetVar("non.a.gr.int", 0.1f);
            SetVar("non.dest.gr.infl", 0.2f);
            SetVar("non.dest.absorp.g.exp.g", 0.05f);
            SetVar("reform.level", 0.3f);
            SetVar("pensioner.q", 0.18f);
            SetVar("publ.inv.norm.q", 0.05f);
            SetVar("secur.norm.q", 0.1f);
            SetVar("tax.soc.opt.q", 0.3f);
            SetVar("retrain.cost.q", 0.5f);
            SetVar("wm.interest-ext.deb.ser.q", 0.01f);

            CurrentStep = 1;
            gridWrapper.AddColum(0);
        }

        public void SetZeroYearInstrVariables(List<float> variables)
        {
            for (int i = endVars.Count, j = 0; i < endVars.Count + instrVars.Count; i++, j++)
            {
                gridWrapper[i][0] = variables[j].ToString();
            }

            gridWrapper.Apply();
        }

        private enum Step
        {
            Current,
            Previous
        }

        public void SimulateNextStep()
        {
            SimulateStep();

            gridWrapper.AddColum(CurrentStep);
            CurrentStep++;
        }

        public void SimulateStep()
        {
            // const // тут может цикл какой-то можно сделать, чтобы не вручную переписывать все константы?
            foreach (var variable in constVars)
            {
                SetVar(variable.Name, GetVar(variable.Name, Step.Previous));
            }

            foreach (var variable in instrVars)
            {
                SetVar(variable.Name, GetVar(variable.Name, Step.Previous));
            }

            // inter
            SetVar("POPUL", GetVar("POPUL", Step.Previous) * (1 + GetVar("popul.g", Step.Previous)));
            SetVar("DEVAL.EXPECT", -GetVar("DEVAL.REAL", Step.Previous) * GetVar("deval.expect-deval.real0", Step.Previous));
            SetVar("EXT.DEBT.PUBL.$", GetVar("EXT.DEBT.PUBL.$", Step.Previous) + GetVar("CAP.MOV.PUBL.INDUCED.$", Step.Previous));

            SetVar("DEF.PUBL.ENTERPR", GetVar("DEF.PUBL.ENTERPR", Step.Previous) * (1 - GetVar("INCR.PRIV.Q", Step.Previous)) * (1 - GetVar("INCR.RESTRUCT.Q", Step.Previous)) * (1 + GetVar("INFLAT.R", Step.Previous)) * (1 - GetVar("PROGR.R", Step.Previous)));

            SetVar("NAT.DEBT", GetVar("NAT.DEBT", Step.Previous) + GetVar("BUDG.DEF.TOT", Step.Previous));
            SetVar("NAT.DEBT.SER", GetVar("NAT.DEBT", Step.Previous) + GetVar("INTEREST.NR", Step.Current));
            SetVar("PUBL.INFRA.DEPREC.$", GetVar("PUBL.INFRA.ASSETS.$", Step.Previous) * GetVar("publ.infra.deprec", Step.Current));
            SetVar("PUBL.INV", GetVar("PUBL.INV", Step.Previous) * (1 + GetVar("PUBL.INV.G", Step.Current)));
            SetVar("RESTRUCT.EXPEND", GetVar("INCR.RESTRUCT.Q", Step.Current) * GetVar("DEF.PUBL.ENTERPR", Step.Previous) * GetVar("restruct.cost-def", Step.Current));

            SetVar("SECUR.EXPEND", GetVar("SECUR.EXPEND", Step.Previous) * (1 + GetVar("SECUR.EXPEND.G", Step.Current)));

            SetVar("SOC.EXPEND", GetVar("SOC.EXPEND", Step.Previous) * (1 + GetVar("SOC.EXPEND.G", Step.Current)));

            SetVar("PRICE.LEVEL.IND", GetVar("PRICE.LEVEL.IND", Step.Previous) * (1 + GetVar("INFLAT.R", Step.Previous)));

            SetVar("INTEREST.RR", ((1 + GetVar("INTEREST.NR", Step.Current)) / (1 + GetVar("INFLAT.R", Step.Previous))) - 1);
            SetVar("MARKET.Q", GetVar("MARKET.Q", Step.Previous) + GetVar("INCR.MARKET.Q", Step.Current));
            SetVar("MARKET.INSTIT.Q", GetVar("MARKET.INSTIT.Q", Step.Previous) + GetVar("IMPROV.INSTIT.Q", Step.Current));
            SetVar("PLAN.Q", 1 - GetVar("MARKET.Q", Step.Current));
            SetVar("PRIV.Q", GetVar("PRIV.Q", Step.Previous) + GetVar("INCR.PRIV.Q", Step.Current));
            SetVar("RESTRUCT.Q", GetVar("RESTRUCT.Q", Step.Previous) + GetVar("INCR.RESTRUCT.Q", Step.Current));

            

            SetVar("PENSIONERS", GetVar("pensioner.q", Step.Current) * GetVar("POPUL", Step.Current));
            SetVar("PENSIONER.INC.PC", GetVar("PENSIONER.INC.PC", Step.Previous) * (1 + GetVar("PENSIONER.INC.PC.G", Step.Current)));
            

            SetVar("UNEMPL.INC.PC", GetVar("UNEMPL.INC.PC", Step.Previous) * (1 + GetVar("UNEMPL.INC.PC.G", Step.Current)));

            SetVar("DEVAL.REAL", (1 + GetVar("DEVAL.R", Step.Current)) / (1 + GetVar("INFLAT.R", Step.Previous)) - 1);
            SetVar("EXCHG.NR", GetVar("EXCHG.NR", Step.Previous) * (1 + GetVar("DEVAL.R", Step.Current)));
            SetVar("EXT.PRIV.ASS.$", (GetVar("EXT.PRIV.ASS.$", Step.Previous) + GetVar("CAP.MOV.PRIV.$", Step.Previous)) * (1 + GetVar("wm.interest.standard", Step.Current)));
            SetVar("IMP.FOR.NONTRAD.$", GetVar("IMP.FOR.NONTRAD.$", Step.Previous) * (1 + GetVar("PROD.NON.TRAD.$.G", Step.Previous)) * (1 - GetVar("DEVAL.REAL", Step.Current) * GetVar("imp-r.deval", Step.Current)) * (1 - (GetVar("IMP.DUTY.R", Step.Current) - GetVar("IMP.DUTY.R", Step.Previous)) * GetVar("imp-customs", Step.Current)));
            SetVar("IMP.FOR.NONTRAD.$G", (GetVar("IMP.FOR.NONTRAD.$", Step.Current) - GetVar("IMP.FOR.NONTRAD.$", Step.Previous)) / GetVar("IMP.FOR.NONTRAD.$", Step.Previous));
            SetVar("IMP.FOR.EXP.$", GetVar("IMP.FOR.EXP.$", Step.Previous) * (1 + GetVar("EXP.NAT.PROD.$G", Step.Previous)) * (1 - GetVar("DEVAL.REAL", Step.Current) * GetVar("imp-r.deval", Step.Current)) * (1 - (GetVar("IMP.DUTY.R", Step.Current) - GetVar("IMP.DUTY.R", Step.Previous)) * GetVar("imp-customs", Step.Current)));
            SetVar("INTEREST.PARITY", GetVar("wm.interest.standard", Step.Current) + GetVar("DEVAL.EXPECT", Step.Current) - GetVar("INTEREST.RR", Step.Current));
            SetVar("BUDG.EXPEND.CONTRACT", GetVar("DEF.PUBL.ENTERPR", Step.Previous) + GetVar("EXT.DEBT.PUBL.SER.RU", Step.Previous) + GetVar("NAT.DEBT.SER", Step.Current));
            SetVar("IMPROV.INSTIT.EXPEND", GetVar("IMPROV.INSTIT.Q", Step.Current) * GetVar("GOV.EXPEND.OFFICER", Step.Previous) * GetVar("retrain.cost.q", Step.Current));
            SetVar("PENSIONER.INC.TOT", GetVar("PENSIONERS", Step.Current) * GetVar("PENSIONER.INC.PC", Step.Current));
            SetVar("PUBL.INFRA.ASSETS.$", GetVar("PUBL.INFRA.ASSETS.$", Step.Previous) - GetVar("PUBL.INFRA.DEPREC.$", Step.Current) + GetVar("PUBL.INV", Step.Current));
            SetVar("EXCHG.VIRT.R", GetVar("PRICE.LEVEL.IND", Step.Current) / 100);
            SetVar("CORP.Q", GetVar("MARKET.Q", Step.Current) - GetVar("PRIV.Q", Step.Current));
            SetVar("SYSTEM", (1 + GetVar("MARKET.Q", Step.Previous) * GetVar("eff.system-market-2", Step.Current) + GetVar("INCR.MARKET.Q", Step.Previous) * GetVar("eff.system-incr.market.-1", Step.Current) + GetVar("INCR.MARKET.Q", Step.Previous) * GetVar("eff.system-incr.market.0", Step.Current) + GetVar("INCR.MARKET.Q", Step.Current) * GetVar("eff.system-incr.market.1", Step.Current) + GetVar("PLAN.Q", Step.Current) * GetVar("eff.system-plan", Step.Current)) * (1 + GetVar("MARKET.INSTIT.Q", Step.Previous) * GetVar("eff.system-matket.instit", Step.Current)) * (1 + GetVar("PRIV.Q", Step.Current) * GetVar("eff.system-priv", Step.Current)) * (1 + GetVar("BANKRUPTCY-Y-N", Step.Current) * GetVar("eff.system-bankrupt", Step.Current)) - 1);

            // IF(E143<=E235,1+E143*E185,1+(E236-E143)*E185)-1+((1/(1+((D13*10)*(D13*10))-D13*10+1)-0.35)/10)+IF(D13>=ABS(10),-E188,IF(D13>=ABS(5),-E187,IF(D13>=ABS(1),-E186,0)))
            SetVar("STABILITY",
                (GetVar("INTEREST.RR", Step.Current) <= GetVar("max.a.gr.int", Step.Current) ? (1f + GetVar("INTEREST.RR", Step.Current) * GetVar("eff.stabil-r.interest", Step.Current)) : (1f + (GetVar("non.a.gr.int", Step.Current) - GetVar("INTEREST.RR", Step.Current)) * GetVar("eff.stabil-r.interest", Step.Current)))
                - 1f + ((1f / (1f + ((GetVar("INFLAT.R", Step.Previous) * 10f) * (GetVar("INFLAT.R", Step.Previous) * 10f)) - GetVar("INFLAT.R", Step.Previous) * 10f + 1f) - 0.35f) / 10f) +
                (GetVar("INFLAT.R", Step.Previous) >= 10 ? -GetVar("eff.stabil-infl>1000", Step.Current) :
                (GetVar("INFLAT.R", Step.Previous) >= 5 ? -GetVar("eff.stabil-infl>500", Step.Current) :
                (GetVar("INFLAT.R", Step.Previous) >= 1 ? -GetVar("eff.stabil-infl>100", Step.Current) : 0))));
            SetVar("STRUCTURE", (float)Math.Exp(GetVar("eff.struct-public.infra", Step.Current) * Math.Log(GetVar("PUBL.INFRA.ASSETS/GDP.Q", Step.Previous))) * (1 + GetVar("RESTRUCT.Q", Step.Previous) * GetVar("eff.struct-san", Step.Current)) * (1 - (GetVar("TAX.TOT.FACT.Q", Step.Previous) - GetVar("tax.soc.opt.q", Step.Current)) * GetVar("eff.struct-tax", Step.Current)) * (1 - GetVar("IMP.DUTY.R", Step.Previous) * GetVar("IMP.$.Q", Step.Previous) * GetVar("eff.struct-customs", Step.Current)) * (1 - GetVar("CORRUPT.CALC", Step.Previous) * GetVar("eff.struct-corrupt", Step.Current)) * (1 + GetVar("FDI.IN.$", Step.Previous) / GetVar("GDP.$", Step.Previous) * GetVar("eff.strukt-fdi", Step.Previous)) - 1);
            SetVar("PROGR.R", (1 + GetVar("SYSTEM", Step.Current)) * (1 + GetVar("STRUCTURE", Step.Current)) * (1 + GetVar("STABILITY", Step.Current)) - 1);
            SetVar("EXP.NAT.PROD.$", GetVar("EXP.NAT.PROD.$", Step.Previous) * (1 - GetVar("exp-time", Step.Current)) * (1 + GetVar("wm.g", Step.Current) * GetVar("exp-wm.g", Step.Current)) * (1 + GetVar("DEVAL.REAL", Step.Current) * GetVar("exp-r.deval", Step.Current)) * (1 - GetVar("comecon.breakoff", Step.Current)) * (1 + GetVar("PROGR.R", Step.Current)));
            SetVar("EXP.NAT.PROD.$G", (GetVar("EXP.NAT.PROD.$", Step.Current) - GetVar("EXP.NAT.PROD.$", Step.Previous)) / GetVar("EXP.NAT.PROD.$", Step.Previous));


            SetVar("EXP.$", GetVar("EXP.NAT.PROD.$", Step.Current) + GetVar("IMP.FOR.EXP.$", Step.Current));
            SetVar("EXP.$.G", (GetVar("EXP.$", Step.Current) - GetVar("EXP.$", Step.Previous)) / GetVar("EXP.$", Step.Previous));
            SetVar("IMP.$", GetVar("IMP.FOR.NONTRAD.$", Step.Current) + GetVar("IMP.FOR.EXP.$", Step.Current));
            SetVar("CUSTOMS.REV", GetVar("IMP.$", Step.Current) * GetVar("EXCHG.NR", Step.Current) * GetVar("IMP.DUTY.R", Step.Current));
            SetVar("PUBL.INV.$", GetVar("PUBL.INV", Step.Current) / GetVar("EXCHG.VIRT.R", Step.Current));
            SetVar("PUBL.INV.$G", (GetVar("PUBL.INV.$", Step.Current) - GetVar("PUBL.INV.$", Step.Previous)) / GetVar("PUBL.INV.$", Step.Previous));
            SetVar("CAP.MOV.WANT.$", -1 * GetVar("INTEREST.PARITY", Step.Current) * GetVar("EXP.$", Step.Current) * GetVar("cap.exp-interest.parity", Step.Current));
            SetVar("UNEMPLOYED", (GetVar("UNEMPLOYED", Step.Previous) * (1 - GetVar("unempl.death.q", Step.Current)) * (1 - (GetVar("GDP.$.G", Step.Previous) - GetVar("non.acc.empl.gr", Step.Current)) * GetVar("employ-growth", Step.Current)) * (1 + (GetVar("IMP.DUTY.R", Step.Previous) - GetVar("IMP.DUTY.R", Step.Current)) * GetVar("IMP.$.Q", Step.Previous) * GetVar("unempl-customs", Step.Current))) + (GetVar("INCR.PRIV.Q", Step.Previous) * GetVar("PERS.WORK.AGE", Step.Previous) * GetVar("unempl-priv", Step.Current)) + (GetVar("BANKRUPTCY-Y-N", Step.Previous) * GetVar("PERS.WORK.AGE", Step.Previous) * GetVar("unempl-bankrupt", Step.Current)) + (GetVar("INCR.RESTRUCT.Q", Step.Previous) * GetVar("PERS.WORK.AGE", Step.Previous) * GetVar("unempl-restruct", Step.Current)));
            if (GetVar("CAP.MOV.WANT.$", Step.Current) < 0)
                SetVar("CAP.MOV.PREVENT.$", GetVar("CAP.MOV.WANT.$", Step.Current) * GetVar("CAP.CONTR.INTENS.Q", Step.Current));
            else
                SetVar("CAP.MOV.PREVENT.$", 0);

            SetVar("CAP.MOV.PRIV.$", GetVar("CAP.MOV.WANT.$", Step.Current) - GetVar("CAP.MOV.PREVENT.$", Step.Current));
            

            // SetVar("EXT.INTEREST.R", D159+ЕСЛИ(D10>E157;(D10-E157)*E245;0));
            if (GetVar("EXT.DEBT.TOT.SER/EXP.Q", Step.Previous) > GetVar("ext.deb.ser.norm.q", Step.Current))
                SetVar("EXT.INTEREST.R", GetVar("wm.interest.standard", Step.Previous) + (GetVar("EXT.DEBT.TOT.SER/EXP.Q", Step.Previous) - GetVar("ext.deb.ser.norm.q", Step.Current)) * GetVar("wm.interest-ext.deb.ser.q", Step.Previous));
            else
                SetVar("EXT.INTEREST.R", GetVar("wm.interest.standard", Step.Previous));

            SetVar("EXT.DEBT.PRIV.$", GetVar("EXT.DEBT.PRIV.$", Step.Previous) + GetVar("CAP.MOV.PRIV.$", Step.Current));


            // SetVar("EXT.DEBT.PRIV.SER.$", =ЕСЛИ(D83>0;D83*E77;0));
            if (GetVar("EXT.DEBT.PRIV.$", Step.Previous) > 0)
                SetVar("EXT.DEBT.PRIV.SER.$", GetVar("EXT.DEBT.PRIV.$", Step.Previous) * GetVar("EXT.INTEREST.R", Step.Current));
            else
                SetVar("EXT.DEBT.PRIV.SER.$", 0);


            SetVar("EXT.DEBT.PUBL.SER.$", GetVar("EXT.DEBT.PUBL.$", Step.Current) * GetVar("EXT.INTEREST.R", Step.Current));


            // SetVar("EXT.AID.$", ЕСЛИ(И(D49<D233;D148>=D239);((D233-D49)/100)*D47*E232;0));
            if (GetVar("GNP.$.IND", Step.Previous) < GetVar("aid.level", Step.Previous) && GetVar("MARKET.Q", Step.Previous) >= GetVar("reform.level", Step.Previous))
                SetVar("EXT.AID.$", ((GetVar("aid.level", Step.Previous) - GetVar("GNP.$.IND", Step.Previous)) / 100) * GetVar("GNP.$", Step.Previous) * GetVar("aid.rate", Step.Current));
            else
                SetVar("EXT.AID.$", 0); ;

            SetVar("EXT.AID.RU", GetVar("EXT.AID.$", Step.Current) * GetVar("EXCHG.NR", Step.Current));


            // SetVar("CAP.CONTR.ADMIN.EXPEND", (E32*E32)*E161*ЕСЛИ(E69>0;E69;0)*E76);
            if (GetVar("CAP.MOV.WANT.$", Step.Current) > 0)
                SetVar("CAP.CONTR.ADMIN.EXPEND", (GetVar("CAP.CONTR.INTENS.Q", Step.Current) * GetVar("CAP.CONTR.INTENS.Q", Step.Current)) * GetVar("admin.cost-cap.control", Step.Current) * GetVar("CAP.MOV.WANT.$", Step.Current) * GetVar("EXCHG.NR", Step.Current));
            else
                SetVar("CAP.CONTR.ADMIN.EXPEND", 0);


            SetVar("CUR.ACC.BAL.$", GetVar("EXP.$", Step.Current) - GetVar("IMP.$", Step.Current) - GetVar("EXT.DEBT.PRIV.SER.$", Step.Current) - GetVar("EXT.DEBT.PUBL.SER.$", Step.Current) + GetVar("EXT.AID.$", Step.Current));
            SetVar("EXT.DEBT.PUBL.SER.RU", GetVar("EXT.DEBT.PUBL.SER.$", Step.Current) * GetVar("EXCHG.NR", Step.Current));
            SetVar("UNEMPL.INC.TOT", GetVar("UNEMPLOYED", Step.Current) * GetVar("UNEMPL.INC.PC", Step.Current));

            // SetVar("FDI.IN.$", E79*ЕСЛИ(E57>0;E57;0));
            SetVar("FDI.IN.$", GetVar("EXP.$", Step.Current) * (GetVar("PROGR.R", Step.Current) > 0 ? GetVar("PROGR.R", Step.Current) : 0));

            
            // SetVar("PROD.NON.TRAD.$", (D55-E230*D79*E163)*(1-IF(D80>=D57+E238,(((D80-D57)*D78+1)*((D80-D57)*D78+1)-1),0))*(1+(D14-0.02)*E164)*(1+(E34-D34)*D97*E165)*(1+E57)*(1+E228*E162));
            SetVar("PROD.NON.TRAD.$",
                (GetVar("PROD.NON.TRAD.$", Step.Previous) - GetVar("comecon.breakoff", Step.Current) * GetVar("EXP.$", Step.Previous) * GetVar("prod-exp.loss", Step.Current)) * 
                (1 - (GetVar("EXP.$.G", Step.Previous) >= (GetVar("PROGR.R", Step.Previous) + GetVar("non.dest.absorp.g.exp.g", Step.Current)) ? (((GetVar("EXP.$.G", Step.Previous) - GetVar("PROGR.R", Step.Previous)) * GetVar("EXP.$.Q", Step.Previous) + 1) * ((GetVar("EXP.$.G", Step.Previous) - GetVar("PROGR.R", Step.Previous)) * GetVar("EXP.$.Q", Step.Previous) + 1) - 1) : 0)) * 
                (1 + (GetVar("BUDG.DEF.TOT/GNP.Q", Step.Previous) - 0.02f) * GetVar("prod-marg.def", Step.Current)) * (1 + (GetVar("IMP.DUTY.R", Step.Current) - GetVar("IMP.DUTY.R", Step.Previous)) * GetVar("IMP.$.Q", Step.Previous) * GetVar("prod-customs", Step.Current)) * (1 + GetVar("PROGR.R", Step.Current)) * (1 + GetVar("popul.g", Step.Current) * GetVar("prod-popul.g", Step.Current)));

            SetVar("PROD.NON.TRAD.$.G", (GetVar("PROD.NON.TRAD.$", Step.Current) - GetVar("PROD.NON.TRAD.$", Step.Previous)) / GetVar("PROD.NON.TRAD.$", Step.Previous));
            SetVar("CAP.MOV.PUBL.INDUCED.$", -(GetVar("CUR.ACC.BAL.$", Step.Current) + GetVar("CAP.MOV.PRIV.$", Step.Current) + GetVar("FDI.IN.$", Step.Current)));
            SetVar("CAP.MOV.PUBL.INDUCED/EXP.Q", GetVar("CAP.MOV.PUBL.INDUCED.$", Step.Current) / GetVar("EXP.$", Step.Current));
            SetVar("CAP.ACC.BAL.$", GetVar("CAP.MOV.PRIV.$", Step.Current) + GetVar("CAP.MOV.PUBL.INDUCED.$", Step.Current) + GetVar("FDI.IN.$", Step.Current));

            


            SetVar("ABSORP.$", GetVar("PROD.NON.TRAD.$", Step.Current) + GetVar("IMP.FOR.NONTRAD.$", Step.Current));
            SetVar("ABSORP.$G", (GetVar("ABSORP.$", Step.Current) - GetVar("ABSORP.$", Step.Previous)) / GetVar("ABSORP.$", Step.Previous));
            SetVar("GDP.$", GetVar("ABSORP.$", Step.Current) + GetVar("EXP.$", Step.Current) - GetVar("IMP.$", Step.Current));
            SetVar("GDP", GetVar("GDP.$", Step.Current) * GetVar("EXCHG.VIRT.R", Step.Current));
            SetVar("GNP.$", GetVar("GDP.$", Step.Current) - GetVar("EXT.DEBT.PUBL.SER.$", Step.Current) - GetVar("EXT.DEBT.PRIV.SER.$", Step.Current) + GetVar("EXT.AID.$", Step.Current));
            SetVar("GNP.$G", (GetVar("GNP.$", Step.Current) - GetVar("GNP.$", Step.Previous)) / GetVar("GNP.$", Step.Previous));
            SetVar("GNP.$.IND", GetVar("GNP.$.IND", Step.Previous) * (1 + GetVar("GNP.$G", Step.Current)));
            SetVar("GNP", GetVar("GNP.$", Step.Current) * GetVar("EXCHG.VIRT.R", Step.Current));
            SetVar("GNP.G", (GetVar("GNP", Step.Current) - GetVar("GNP", Step.Previous)) / GetVar("GNP", Step.Previous));
            SetVar("PCI", GetVar("GNP", Step.Current) / GetVar("POPUL", Step.Current));


            SetVar("PERS.WORK.AGE", GetVar("workers.q", Step.Current) * GetVar("POPUL", Step.Current));
            SetVar("OFFICERS", GetVar("officer.q", Step.Current) * GetVar("PERS.WORK.AGE", Step.Current));
            SetVar("OFFICERS.INC.PC", GetVar("OFFICERS.INC.PC", Step.Previous) * (1 + GetVar("OFFICER.INC.PC.G", Step.Current)));

            SetVar("OFFICER.INC.TOT", GetVar("OFFICERS", Step.Current) * GetVar("OFFICERS.INC.PC", Step.Current));
            SetVar("OFFICER.ADD.COST", GetVar("OFFICER.INC.TOT", Step.Current) * GetVar("officer.sidecost.q", Step.Current));
            SetVar("GOV.EXPEND.OFFICER", GetVar("OFFICER.INC.TOT", Step.Current) + GetVar("OFFICER.ADD.COST", Step.Current));
            SetVar("BUDG.EXPEND.FREE", GetVar("UNEMPL.INC.TOT", Step.Current) + GetVar("GOV.EXPEND.OFFICER", Step.Current) + GetVar("PUBL.INV.G", Step.Current) + GetVar("CAP.CONTR.ADMIN.EXPEND", Step.Current) + GetVar("PENSIONER.INC.TOT", Step.Current) + GetVar("RESTRUCT.EXPEND", Step.Current) + GetVar("SECUR.EXPEND", Step.Current) + GetVar("SOC.EXPEND", Step.Current) + GetVar("IMPROV.INSTIT.EXPEND", Step.Current));
            SetVar("BUDG.EXPEND.FREE.G", (GetVar("BUDG.EXPEND.FREE", Step.Current) - GetVar("BUDG.EXPEND.FREE", Step.Previous)) / GetVar("BUDG.EXPEND.FREE", Step.Previous));
            SetVar("BUDG.EXPEND.TOT", GetVar("BUDG.EXPEND.FREE", Step.Current) + GetVar("BUDG.EXPEND.CONTRACT", Step.Current));
            SetVar("TAX.TOT.FACT.Q", GetVar("TAX.TOT.FACT.Q", Step.Previous) * (1 + (GetVar("OFFICER.INC.PC.G", Step.Current) - GetVar("GNP.G", Step.Current)) * GetVar("tax.fact-officer.inc", Step.Previous)) * (1 + (GetVar("TAX.TOT.R", Step.Current) - GetVar("TAX.TOT.R", Step.Previous)) * GetVar("tax.fact-tax.legal", Step.Current)) * (1 - GetVar("INCR.MARKET.Q", Step.Current) * GetVar("tax.fact-market", Step.Current)) * (1 + GetVar("IMPROV.INSTIT.Q", Step.Previous) * GetVar("tax.fact-instit", Step.Previous)));
            SetVar("TAX.REV", GetVar("GDP", Step.Current) * GetVar("TAX.TOT.FACT.Q", Step.Previous));
            SetVar("BUDG.REV.TOT", GetVar("EXT.AID.RU", Step.Current) + GetVar("TAX.REV", Step.Current) + GetVar("CUSTOMS.REV", Step.Current));
            SetVar("BUDG.DEF.TOT", GetVar("BUDG.EXPEND.TOT", Step.Current) - GetVar("BUDG.REV.TOT", Step.Current));
            SetVar("BUDG.DEF.PRIM", GetVar("BUDG.DEF.TOT", Step.Current) - GetVar("EXT.DEBT.PUBL.SER.RU", Step.Current) - GetVar("NAT.DEBT.SER", Step.Current));
            SetVar("BUDG.DEF.TOT/GNP.Q", GetVar("BUDG.DEF.TOT", Step.Current) / GetVar("GNP", Step.Current));
            // SetVar("INFLAT.R", ЕСЛИ(D13<=0;D13+E14*E209;D13*(1+E31*D97*E206)*(1+E14*E208)*(1+(E14-D14)*E207)*(1-E143*E211)*(1+E27*E210)*(1+(E137-D137)*E212)*(1+(E34-D34)*D97*D213)));
            SetVar("INFLAT.R", GetVar("INFLAT.R", Step.Previous) <= 0 ?
                (GetVar("INFLAT.R", Step.Previous) + GetVar("BUDG.DEF.TOT/GNP.Q", Step.Current) * GetVar("infl.abs-abs.def", Step.Current)) :
                (GetVar("INFLAT.R", Step.Previous) * (1 + GetVar("DEVAL.R", Step.Current) * GetVar("IMP.$.Q", Step.Previous) * GetVar("infl-r.deval", Step.Current)) * (1 + GetVar("BUDG.DEF.TOT/GNP.Q", Step.Current) * GetVar("infl-abs.def", Step.Current)) * (1 + (GetVar("BUDG.DEF.TOT/GNP.Q", Step.Current) - GetVar("BUDG.DEF.TOT/GNP.Q", Step.Previous)) * GetVar("infl-marg.def", Step.Current)) * (1 - GetVar("INTEREST.RR", Step.Current) * GetVar("infl-r.interest", Step.Current)) * (1 + GetVar("INCR.MARKET.Q", Step.Current) * GetVar("infl-incr.market", Step.Current)) * (1 + (GetVar("TAX.TOT.FACT.Q", Step.Current) - GetVar("TAX.TOT.FACT.Q", Step.Previous)) * GetVar("infl-tax.fact", Step.Current)) * (1 + (GetVar("IMP.DUTY.R", Step.Current) - GetVar("IMP.DUTY.R", Step.Previous)) * GetVar("IMP.$.Q", Step.Previous) * GetVar("infl-customs", Step.Previous))));

            SetVar("OFFICERS.INC.R.GR", (1 + GetVar("OFFICER.INC.PC.G", Step.Current)) / (1 + GetVar("INFLAT.R", Step.Current)) - 1);
            

            SetVar("BUDG.EXPEND.TOT.G", (GetVar("BUDG.EXPEND.TOT", Step.Current) - GetVar("BUDG.EXPEND.TOT", Step.Previous)) / GetVar("BUDG.EXPEND.TOT", Step.Previous));
            SetVar("GOV.EXPEND.OFFICER/EXPEND", GetVar("GOV.EXPEND.OFFICER", Step.Current) / GetVar("BUDG.EXPEND.TOT", Step.Current));
            SetVar("NAT.DEBT.SER/BUDG.EXPEND.TOT.Q", GetVar("NAT.DEBT.SER", Step.Current) / GetVar("BUDG.EXPEND.TOT", Step.Current));
            SetVar("PENSIONER.INC.TOT/EXPEND.TOT.Q", GetVar("PENSIONER.INC.TOT", Step.Current) / GetVar("BUDG.EXPEND.TOT", Step.Current));
            SetVar("PUBL.INV/EXPEND.TOT.Q", GetVar("PUBL.INV", Step.Current) / GetVar("BUDG.EXPEND.TOT", Step.Current));
            SetVar("SECUR.EXPEND/EXPEND.Q", GetVar("SECUR.EXPEND", Step.Current) / GetVar("BUDG.EXPEND.TOT", Step.Current));
            SetVar("SOC.EXPEND/EXPEND.Q", GetVar("SOC.EXPEND", Step.Current) / GetVar("BUDG.EXPEND.TOT", Step.Current));
            SetVar("UNEMPL.INC.TOT/EXPEND.TOT.G", GetVar("UNEMPL.INC.TOT", Step.Current) / GetVar("BUDG.EXPEND.TOT", Step.Current));
            SetVar("SECUR.EXPEND.Q", GetVar("SECUR.EXPEND", Step.Current) / GetVar("GNP", Step.Current));
            SetVar("OFFICER.INC.Q", GetVar("OFFICERS.INC.PC", Step.Current) / GetVar("PCI", Step.Current));

            

            SetVar("CORRUPT.CALC", (1 + (GetVar("MARKET.Q", Step.Current) - GetVar("MARKET.INSTIT.Q", Step.Current)) * GetVar("corrupt-instit", Step.Current)) * (1 + (GetVar("officer.inc.norm.q", Step.Current) - GetVar("OFFICER.INC.Q", Step.Current)) * GetVar("corrupt-officer.inc", Step.Current)) * (1 + (GetVar("secur.norm.q", Step.Current) - GetVar("SECUR.EXPEND.Q", Step.Current)) * GetVar("corrupt-security", Step.Current)) - 1);
            // SetVar("CORRUPT.FACTOR", ЕСЛИ(E146<0;0;E146));
            SetVar("CORRUPT.FACTOR", GetVar("CORRUPT.CALC", Step.Current) < 0 ? 0 : GetVar("CORRUPT.CALC", Step.Current));


            SetVar("PCI.$", GetVar("GNP.$", Step.Current) / GetVar("POPUL", Step.Current));
            SetVar("PCI.$.G", (GetVar("PCI.$", Step.Current) - GetVar("PCI.$", Step.Previous)) / GetVar("PCI.$", Step.Previous));
            SetVar("EXP.$.Q", GetVar("EXP.$", Step.Current) / GetVar("GDP.$", Step.Current));
            SetVar("EXT.DEBT.PUBL.SER/EXPEND.TOT.Q", GetVar("EXT.DEBT.PUBL.SER.RU", Step.Current) / GetVar("BUDG.EXPEND.TOT", Step.Current));
            SetVar("EXT.DEBT.PUBLIC.SER.Q", GetVar("EXT.DEBT.PUBL.SER.RU", Step.Current) / GetVar("GDP", Step.Current));
            SetVar("EXT.DEBT.PUBL/GDP$.Q", GetVar("EXT.DEBT.PUBL.$", Step.Current) / GetVar("GDP.$", Step.Current));
            SetVar("FDI.IN./GDP.Q", GetVar("FDI.IN.$", Step.Current) / GetVar("GDP.$", Step.Current));
            SetVar("IMP.$.Q", GetVar("IMP.$", Step.Current) / GetVar("GDP.$", Step.Current));
            SetVar("BUDG.EXPEND.TOT/GNP.Q", GetVar("BUDG.EXPEND.TOT", Step.Current) / GetVar("GNP", Step.Current));
            SetVar("NAT.DEBT/GNP.Q", GetVar("NAT.DEBT", Step.Current) / GetVar("GNP", Step.Current));
            SetVar("PUBL.INV.Q", GetVar("PUBL.INV.$", Step.Current) / GetVar("GNP.$", Step.Current));

            //End
            SetVar("GDP.$.G", (GetVar("GDP.$", Step.Current) - GetVar("GDP.$", Step.Previous)) / GetVar("GDP.$", Step.Previous));
            SetVar("GDP.$.IND", GetVar("GDP.$.IND", Step.Previous) * (1 + GetVar("GDP.$.G", Step.Current)));
            SetVar("PCI.$.IND", GetVar("PCI.$.IND", Step.Previous) * (1 + GetVar("PCI.$.G", Step.Current)));
            SetVar("BELT.TIGHT.IND", GetVar("GNP.$.IND", Step.Current) / GetVar("GDP.$.IND", Step.Current) * 100);
            SetVar("PUBL.INFRA.ASSETS/GDP.Q", GetVar("PUBL.INFRA.ASSETS.$", Step.Current) / GetVar("GDP.$", Step.Current));
            
            SetVar("EXT.DEBT.TOT.SER/EXP.Q", (GetVar("EXT.DEBT.PUBL.SER.$", Step.Current) + GetVar("EXT.DEBT.PRIV.SER.$", Step.Current)) / GetVar("EXP.$", Step.Current));
            SetVar("EXT.DEBT.PUBL/GDP.$.Q", GetVar("EXT.DEBT.PUBL.$", Step.Current) / GetVar("GDP.$", Step.Current));
            SetVar("IMP.SURPL.$", GetVar("IMP.$", Step.Current) - GetVar("EXP.$", Step.Current));

            SetVar("UNEMPL.Q", GetVar("UNEMPLOYED", Step.Current) / GetVar("PERS.WORK.AGE", Step.Current));
            SetVar("BUDG.DEF.PRIM./GNP.Q", GetVar("BUDG.DEF.PRIM", Step.Current) / GetVar("GNP", Step.Current));
            SetVar("NAT.DEBT/GNP.Q ", GetVar("NAT.DEBT", Step.Current) / GetVar("GNP", Step.Current));
            SetVar("NAT.DEBT.SER/GNP.Q", GetVar("NAT.DEBT.SER", Step.Current) / GetVar("GNP", Step.Current));
            SetVar("UNEMPL.INC.Q", GetVar("UNEMPL.INC.PC", Step.Current) / GetVar("PCI", Step.Current));
            SetVar("PENSIONERS.INC.Q", GetVar("PENSIONER.INC.PC", Step.Current) / GetVar("PCI", Step.Current));
            SetVar("SOC.EXPEND.Q", GetVar("SOC.EXPEND", Step.Current) / GetVar("GNP", Step.Current));
            SetVar("GINI.Q", GetVar("GINI.Q", Step.Previous) + (GetVar("CORP.Q", Step.Current) - GetVar("CORP.Q", Step.Previous)) * GetVar("gini-delta.corp", Step.Previous) + GetVar("INCR.MARKET.Q", Step.Previous) * GetVar("gini-delta.market", Step.Previous) - GetVar("IMPROV.INSTIT.Q", Step.Previous) * GetVar("gini-delta.instit", Step.Previous) - (GetVar("UNEMPL.INC.Q", Step.Current) - GetVar("UNEMPL.INC.Q", Step.Previous)) * GetVar("gini-unemploy.inc", Step.Current) - (GetVar("PENSIONERS.INC.Q", Step.Current) - GetVar("PENSIONERS.INC.Q", Step.Previous)) * GetVar("gini-pensioners.inc", Step.Current) - (GetVar("SOC.EXPEND.Q", Step.Current) - GetVar("SOC.EXPEND.Q", Step.Previous)) * GetVar("gini-soc.expend", Step.Current));
            SetVar("UNEMPL.INC.MIN.Q", GetVar("OFFICER.INC.Q", Step.Previous) * GetVar("unempl.y-officers.inc", Step.Previous));
            
            
        }


        public bool IsVariableInstr(string name)
        {
            int id = variableIds[name];
            if (id >= endVars.Count && id < endVars.Count + instrVars.Count)
                return true;

            return false;
        }

        private float GetVar(string name, Step step)
        {
            if (step == Step.Current)
            {
                // BIG COSTIL
                if (gridWrapper[variableIds[name]][CurrentStep] == null)
                {
                    Trace.WriteLine(name);
                }

                return float.Parse(gridWrapper[variableIds[name]][CurrentStep]);
            }
            else
            {

                return float.Parse(gridWrapper[variableIds[name]][CurrentStep - 1]);
            }
        }

        private void SetVar(string name, float value)
        {
            gridWrapper[variableIds[name]][CurrentStep] = value.ToString();
        }

        public void MoveCarriageTo(int to)
        {
            if (CurrentStep == to)
                return;

            else if (CurrentStep < to)
            {
                while (CurrentStep != to)
                {
                    gridWrapper.AddColum(CurrentStep++);
                }
                return;
            }

            else
            {
                while (CurrentStep != to)
                {
                    gridWrapper.PopColum();
                    CurrentStep--;
                }
                return;
            }
        }

    }
}
