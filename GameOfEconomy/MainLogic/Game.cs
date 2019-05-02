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
                new EEndVar { Name = "End.Var1", LongName = "Какая то хуйня" },
                new EEndVar { Name = "End.Var2", LongName = "Какая то хуйня" },
                new EEndVar { Name = "End.Var3", LongName = "Какая то хуйня" }
            };

            instrVars = new List<EInstrVar>
            {
                new EInstrVar { Name = "Instr.Var1", LongName = "Какая то хуйня" },
                new EInstrVar { Name = "Instr.Var2", LongName = "Какая то хуйня" },
                new EInstrVar { Name = "Instr.Var3", LongName = "Какая то хуйня" }
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
