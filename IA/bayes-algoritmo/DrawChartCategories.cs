using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace IA.detectarIdioma
{
    public class DrawChartCategories
    {
        StringBuilder str = new StringBuilder();

        public string BindChart(DataTable data, string div, string titleup, string titledown)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = data;

                str.Append(@"<script type=text/javascript> google.load( *visualization*, *1*, {packages:[*corechart*]});
                       google.setOnLoadCallback(drawChart);
                       function drawChart() {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Categorias');
        data.addColumn('number', 'Porcentaje');      

        data.addRows(" + dt.Rows.Count + ");");

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    str.Append("data.setValue( " + i + "," + 0 + "," + "'" + dt.Rows[i]["categoria"].ToString() + "');");
                    str.Append("data.setValue(" + i + "," + 1 + "," + dt.Rows[i]["porcentaje"].ToString() + ") ;");
                }

                str.Append(" var chart = new google.visualization.ColumnChart(document.getElementById('" + div + "'));");
                str.Append(" chart.draw(data, {width: 650, height: 300, title: '" + titleup + "',");
                str.Append("hAxis: {title: '" + titledown + "', titleTextStyle: {color: 'green'}}");
                str.Append("}); }");
                str.Append("</script>");

                return str.ToString().TrimEnd(',').Replace('*', '"');
            }
            catch
            {
                return "error";
            }
        }
    }
}