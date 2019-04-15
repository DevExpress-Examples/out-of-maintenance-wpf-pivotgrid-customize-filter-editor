Imports DevExpress.Data.Filtering
Imports DevExpress.Xpf.Core.FilteringUI
Imports DevExpress.Xpf.Editors
Imports DevExpress.Xpf.Editors.Settings
Imports System
Imports System.Windows

Namespace WpfPivotCustomizeFilterEditorExample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits DevExpress.Xpf.Core.ThemedWindow

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub ThemedWindow_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
			pivotGridControl1.BestFitArea = DevExpress.Xpf.PivotGrid.FieldBestFitArea.FieldHeader
			pivotGridControl1.BestFit()
		End Sub

		Private Sub FilterEditorControl_QueryOperators(ByVal sender As Object, ByVal e As FilterEditorQueryOperatorsEventArgs)
			If e.FieldName = "fieldOrderDate" Then
				e.Operators.Clear()
				e.Operators.Add(New FilterEditorOperatorItem(FilterEditorOperatorType.Between) With {.Caption = "Between"})
				e.Operators.Add(New FilterEditorOperatorItem(FilterEditorOperatorType.NotBetween) With {.Caption = "NotBetween"})
				e.Operators.Add(CreateLastYearsOperator())
			End If
		End Sub
		Private Function CreateLastYearsOperator() As FilterEditorOperatorItem
			Const CustomFunctionName As String = "LastYears"
			Dim currentYear = Date.Now.Year
			Dim customFunction As ICustomFunctionOperatorBrowsable = CustomFunctionFactory.Create(CustomFunctionName, Function([date] As Date, threshold As Integer)
				Return currentYear >= [date].Year AndAlso currentYear - [date].Year <= threshold
			End Function)
			CriteriaOperator.RegisterCustomFunction(customFunction)
			Dim customFunctionEditSettings = New BaseEditSettings() {
				New TextEditSettings With {.MaskType = MaskType.Numeric, .Mask = "D", .MaskUseAsDisplayFormat = True, .NullText ="Enter the number of years before..."}
			}
			Return New FilterEditorOperatorItem(CustomFunctionName, customFunctionEditSettings) With {.Caption = "Last Years"}
		End Function


	End Class
End Namespace
