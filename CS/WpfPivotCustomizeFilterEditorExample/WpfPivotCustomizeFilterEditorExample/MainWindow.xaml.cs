using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core.FilteringUI;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using System;
using System.Windows;

namespace WpfPivotCustomizeFilterEditorExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DevExpress.Xpf.Core.ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            pivotGridControl1.BestFitArea = DevExpress.Xpf.PivotGrid.FieldBestFitArea.FieldHeader;
            pivotGridControl1.BestFit();
        }

        private void FilterEditorControl_QueryOperators(object sender, FilterEditorQueryOperatorsEventArgs e)
        {
            if (e.FieldName == "fieldOrderDate")
            {
                e.Operators.Clear();
                e.Operators.Add(new FilterEditorOperatorItem(FilterEditorOperatorType.Between) { Caption = "Between" });
                e.Operators.Add(new FilterEditorOperatorItem(FilterEditorOperatorType.NotBetween) { Caption = "NotBetween" });
                e.Operators.Add(CreateLastYearsOperator());
            }
        }
        private FilterEditorOperatorItem CreateLastYearsOperator()
        {
            const string CustomFunctionName = "LastYears";
            var currentYear = DateTime.Now.Year;
            ICustomFunctionOperatorBrowsable customFunction = CustomFunctionFactory.Create(CustomFunctionName,
                (DateTime date, int threshold) =>
                {
                    return currentYear >= date.Year && currentYear - date.Year <= threshold;
                }
                );
            CriteriaOperator.RegisterCustomFunction(customFunction);
            var customFunctionEditSettings = new BaseEditSettings[] {
                new TextEditSettings {
                    MaskType = MaskType.Numeric,
                    Mask = "D",
                    MaskUseAsDisplayFormat = true,
                    NullText ="Enter the number of years before..."}
            };
            return new FilterEditorOperatorItem(CustomFunctionName, customFunctionEditSettings) { Caption = "Last Years" };
        }


    }
}
