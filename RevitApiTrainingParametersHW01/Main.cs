using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiTrainingParametersHW01    //Создайте приложение, которое выбирает несколько стен по граням и выводит в окне общий объём выбранных стен.
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
       {
            UIApplication uiapp = commandData.Application;  //Обратился к rvt
            UIDocument uidoc = uiapp.ActiveUIDocument;  //Обратился к интетрфейсу тек документа
            Document doc = uidoc.Document;  //обр к базе данных самого документа

            var elementlist = new List<Element>(); //создание пустого списка стен
            var parameterLists = new List<Parameter>();  //пустой список параметров

            IList<Reference> selectedRefList = uidoc.Selection.PickObjects(ObjectType.Face, "Выберите стены");
            foreach (var selectedElements in selectedRefList) 
            {
                Element element = doc.GetElement(selectedElements); //преобразование в Element
                elementlist.Add(element);

                foreach (var parameter in elementlist)
                {
                    if (parameter is Wall)    //проверка категории
                    {
                        Parameter volumeParameters = parameter.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED); //получение значения параметров
                        parameterLists.Add(volumeParameters); //заполнения списка
                        var sum = 0;
                        var valueParameters = new List<int>();   //пустой список для значения параметров
                        foreach (var i in parameterLists)
                        {
                            var value = Convert.ToInt16(i); //приведение типов
                            valueParameters.Add(value); //Заполнение списка значений
                            //sum += i;
                        }
                        foreach (var c in valueParameters)
                        {
                            sum += c;
                        }

                        TaskDialog.Show("Объём выбраных стен = ", sum.ToString());

                    }
                }
            }
            return Result.Succeeded;
        }
    }

}
