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
                new EInterVar { Name = "Inter.Var1", LongName = "Какая то хуйня" },
                new EInterVar { Name = "Inter.Var2", LongName = "Какая то хуйня" },
                new EInterVar { Name = "Inter.Var3", LongName = "Какая то хуйня" }
            };

            constVars = new List<EConstVar>
            {
                new EConstVar { Name = "Const.Var1", LongName = "Какая то хуйня" },
                new EConstVar { Name = "Const.Var2", LongName = "Какая то хуйня" },
                new EConstVar { Name = "Const.Var3", LongName = "Какая то хуйня" }
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
