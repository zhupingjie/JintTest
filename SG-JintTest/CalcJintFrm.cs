using Jint;
using Jint.Parser;
using Jint.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SG_JintTest
{
    public partial class CalcJintFrm : Form
    {
        public CalcJintFrm()
        {
            InitializeComponent();
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            this.richTextBox1.SelectAll();
        }

        string GetFunc()
        {
            return @"function sin(num)
{
    return Math.sin(num);
}
function cos(num)
{
    return Math.cos(num);
}
function tan(num) {
    return Math.tan(num);
}
function Asin(num) {
    return Math.asin(num);
}
function Acos(num) {
    return Math.acos(num);
}
function Atan(num) {
    return Math.atan(num);
}
function ROUNDDOWN(num1, num2) {
    return Math.floor(num1 * Math.pow(10, num2)) / Math.pow(10, num2);
}
function ROUNDUP(num1, num2) {
    return Math.ceil(num1 * Math.pow(10, num2)) / Math.pow(10, num2);
}

function CEILING(num1, num2) {
    if (num2 == 0) return num2;

    var m = num1 / num2;
    var n = Math.ceil(m);
    return num2 * n;
}
function FLOOR(num1, num2)
{
    if (num2 == 0) return num2;

    var m = num1 / num2;
    var n = Math.floor(m);
    return num2 * n;
}
function cont(num)
{
    for (var i= 1; i < arguments.length; i++)
    {
        if (num == arguments[i])
            return true;
    }
    return false;
}
function Pi() {
    return Math.PI;
}
function Abs(num) {
    return Math.abs(num);
}
function INT(num) {
    return parseInt(num);
}
function log(msg) {
    throw 'Error:' + msg;
}
function GetDate() {
    var date = new Date();
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();

    return y + '' + (m.length == 1 ? '0' + m : m) + '' + (d.length == 1 ? '0' + d : d);
}";
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            Engine engine = new Engine();
            var exp = this.richTextBox1.Text.Trim();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                DataGridViewCell cell1 = row.Cells[0];
                DataGridViewCell cell2 = row.Cells[1];

                if (cell1.Value == null || cell2.Value == null) continue;

                var code = cell1.Value.ToString();
                var value = cell2.Value.ToString();

                if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(value))
                {
                    try
                    {
                        engine.SetValue(code, value);
                    }
                    catch (Exception ex)
                    {
                        this.richTextBox2.Text = string.Format("设置参数[{0}]的值[{1}]错误", code, value);
                        return;
                    }
                }
            }
            try
            {
                engine.Execute(GetFunc() + " " + exp);
                var result = engine.GetCompletionValue();
                if (result == null || result.ToString() == "undefined")
                {
                    this.richTextBox2.Text = "值为空";
                }
                else
                {
                    this.richTextBox2.Text = result.ToString();
                }
            }
            catch (ParserException px)
            {
                this.richTextBox2.Text = "语法错误:" + px.Message;
            }
            catch (JavaScriptException jx)
            {
                if (jx.Message.Contains("is not defined"))
                {
                    var pname = jx.Message.Replace(" is not defined", "");
                    this.richTextBox2.Text = string.Format("参数[{0}]尚未赋值", pname);
                }
                else
                {
                    this.richTextBox2.Text = "语法错误";
                }
            }
            catch(Exception ex)
            {
                this.richTextBox2.Text = "未知错误:" + ex.Message;
            }
        }
    }
}
