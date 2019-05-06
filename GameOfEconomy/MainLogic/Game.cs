using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfEconomy.MainLogic
{
    public class Game
    {
        private int CurrentStep;

        private DataGridWrapper gridWrapper;

        private List<EEndVar> endVars;
        public  List<EInstrVar> instrVars;
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
		new EEndVar { Name = "NAT.DEBT/GNP.Q", LongName = "Общественный долг по отношению к ВНП" },
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
                new EInstrVar { Name = "INCR.PRIV.Q", LongName = "Увеличение доли приватизированных предприятий" },
                new EInstrVar { Name = "INCR.MARKET.Q", LongName = "Увеличение доли рыночной экономики" },
                new EInstrVar { Name = "IMPROV.INSTIT.Q", LongName = "Улучшение уровня образованности" },
                new EInstrVar { Name = "INCR.RESTRUCT.Q", LongName = "Увеличение уровня реорганизации" },
                new EInstrVar { Name = "BANKRUPTCY-Y-N", LongName = "Применение закона о банкротстве (да - нет)" },
                new EInstrVar { Name = "DEVAL.R", LongName = "Уровень девальвации" },
                new EInstrVar { Name = "CAP.CONTR.INTENS.Q", LongName = "Интенсивность управления выводом капитала" },
                new EInstrVar { Name = "INTEREST.NR", LongName = "Номинальная процентная ставка" },
                new EInstrVar { Name = "IMP.DUTY.R", LongName = "Уровень импортных пошлин" },
		new EInstrVar { Name = "TAX.TOT.R", LongName = "Общий уровень налогов" },
                new EInstrVar { Name = "PUBL.INV.G", Темп прироста общественных вложений" },
                new EInstrVar { Name = "UNEMPL.INC.PC.G", LongName = "Темп прироста доходов безработных на душу населения" },
                new EInstrVar { Name = "OFFICER.INC.PC.G", LongName = "Темп прироста доходов служащих на душу населения" },
                new EInstrVar { Name = "PENSIONER.INC.PC.G", LongName = "Темп прироста доходов пенсионеров на душу населения" },
                new EInstrVar { Name = "SOC.EXPEND.G", LongName = "Темп прироста социальных расходов" },
                new EInstrVar { Name = "SECUR.EXPEND.G", LongName = "Темп прироста расходов на безопасность" }
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
                new EConstVar { Name = "Ceff.system-incr.market.0", LongName = "Воздействие усиления системы рынка в периоде 0 на эффективность" },
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
                new EConstVar { Name = "eff-neg.inflat", LongName = "Воздействие "отрицательной" инфляции (дефляция) на эффективность" },
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
            // end
            gridWrapper[0][0] = "113";
            gridWrapper[1][0] = "142";
            gridWrapper[2][0] = "14.1";

            // instr
            gridWrapper[3][0] = "24";
            gridWrapper[4][0] = "1942";
            gridWrapper[5][0] = "24";

            // inter
            gridWrapper[6][0] = "1.01";
            gridWrapper[7][0] = "24.1";
            gridWrapper[8][0] = "1.24";

            // const
            gridWrapper[9][0] = "5";
            gridWrapper[10][0] = "6";
            gridWrapper[11][0] = "2";

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
            // const
            SetVar("Const.Var1", GetVar("Const.Var1", Step.Previous));
            SetVar("Const.Var2", GetVar("Const.Var2", Step.Previous));
            SetVar("Const.Var3", GetVar("Const.Var3", Step.Previous));

            // inter
            SetVar("Inter.Var1", GetVar("Inter.Var1", Step.Previous) + GetVar("Const.Var1", Step.Previous));
            SetVar("Inter.Var2", GetVar("Inter.Var2", Step.Previous) * GetVar("Const.Var2", Step.Previous) + GetVar("Inter.Var1", Step.Current));
            SetVar("Inter.Var3", GetVar("Inter.Var3", Step.Previous) + 2);

            // instr
            SetVar("Instr.Var1", GetVar("Instr.Var1", Step.Previous));
            SetVar("Instr.Var2", GetVar("Instr.Var2", Step.Previous));
            SetVar("Instr.Var3", GetVar("Instr.Var3", Step.Previous));

            //End
            SetVar("End.Var1", 
                GetVar("Instr.Var1", Step.Previous) * GetVar("Inter.Var1", Step.Previous) * GetVar("Instr.Var2", Step.Current)
                );

            SetVar("End.Var2",
                GetVar("Instr.Var2", Step.Previous) + GetVar("Const.Var1", Step.Previous) - GetVar("Instr.Var2", Step.Previous) + GetVar("End.Var2", Step.Previous)
                );

            SetVar("End.Var3",
                GetVar("Instr.Var3", Step.Previous) + GetVar("Instr.Var1", Step.Previous) - GetVar("Instr.Var1", Step.Previous) + GetVar("End.Var2", Step.Previous)
                );


            gridWrapper.AddColum(CurrentStep);
            CurrentStep++;
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
                return float.Parse(gridWrapper[variableIds[name]][CurrentStep]);
            else
                return float.Parse(gridWrapper[variableIds[name]][CurrentStep - 1]);
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
