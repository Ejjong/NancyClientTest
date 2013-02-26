using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GenerateGroupBoxs();

            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            ci = Thread.CurrentThread.CurrentUICulture;
        }

        private void GenerateGroupBoxs()
        {
            var groupInfo = GetGroupInfo();

            var lv1s = groupInfo.GroupBy(g => g.Value.Item1).Where(i => i.Key != null);

            foreach (var lv1 in lv1s)
            {
                bool isFirstlv3 = true;
                var lv1Group = new GroupBox() { Header = lv1.Key };
                var lv2s = lv1.GroupBy(l1 => l1.Value.Item2).Where(i => i.Key != null);
                if (!lv2s.Any())
                {
                }
                else
                {
                    var tempDic2 = new Dictionary<string, object>();
                    WrapPanel wp2 = null;
                    foreach (var lv2 in lv2s)
                    {
                        bool isFirstlv4 = true;
                        var lv3s = lv2.GroupBy(l2 => l2.Value.Item3).Where(i => i.Key != null);
                        if (!lv3s.Any())
                        {
                            if (isFirstlv3)
                            {
                                wp2 = new WrapPanel();
                                isFirstlv3 = false;
                                lv1Group.Content = wp2;
                            }
                            else
                            {
                                wp2 = lv1Group.Content as WrapPanel;
                            }
                            wp2.Children.Add(new CheckBox() { Content = lv2.Key, Tag = lv2.Single().Key});
                        }
                        else
                        {
                            if (!tempDic2.ContainsKey(lv2.Key))
                            {
                                tempDic2.Add(lv2.Key, new GroupBox() { Header = lv2.Key });
                            }
                            var lv2Group = tempDic2[lv2.Key] as GroupBox;

                            WrapPanel wp3 = null;
                            foreach (var lv3 in lv3s)
                            {
                                var lv4s = lv3.GroupBy(l3 => l3.Value.Item4).Where(i => i.Key != null);
                                if (!lv4s.Any())
                                {
                                    if (isFirstlv4)
                                    {
                                        wp3 = new WrapPanel();
                                        isFirstlv4 = false;
                                        lv2Group.Content = wp3;
                                    }
                                    else
                                    {
                                        wp3 = lv2Group.Content as WrapPanel;
                                    }
                                    wp3.Children.Add(new CheckBox() { Content = lv3.Key, Tag = lv3.Single().Key });
                                }
                                else
                                {

                                }

                            }
                        }
                    }
                    var tempWp = (wp2 == null) ? new WrapPanel() : wp2;
                    foreach (var d in tempDic2)
                    {
                        tempWp.Children.Add(d.Value as GroupBox);
                    }
                    lv1Group.Content = tempWp;
                    sp.Children.Add(lv1Group);
                }
            }
        }

        private Dictionary<string, Tuple<string, string, string, string, string>> GetGroupInfo()
        {
            var ret = new Dictionary<string, Tuple<string, string, string, string, string>>();

            ret.Add("2001100009", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring", "Pupil size(Lt)", null, null
                                      ));
            ret.Add("2001100020", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring", "Pupil size(Rt)", null, null
                                      ));
            ret.Add("2001100021", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring", "Reflex(Rt: pupil)", null, null
                                      ));
            ret.Add("2001100048", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring", "Position", null, null
                                      ));
            ret.Add("2001100063", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring", "Reflex(Knee-jerk)", null, null
                                      ));
            ret.Add("2001100065", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring", "Pupil(Lt) size", null, null
                                      ));
            ret.Add("2001100066", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring", "Pupil(Rt) size", null, null
                                      ));
            ret.Add("2001100067", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring", "Pupil(Lt) Reflex", null, null
                                      ));
            ret.Add("2001100068", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring", "Pupil(Rt) Reflex", null, null
                                      ));
            ret.Add("3001100065", new Tuple<string, string, string, string, string>(

                                    "CONDITION", "Monitoring2", "Pupil(Lt) size", null, null
                                    ));
            ret.Add("3001100066", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring2", "Pupil(Rt) size", null, null
                                      ));
            ret.Add("3001100067", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring2", "Pupil(Lt) Reflex", null, null
                                      ));
            ret.Add("3001100068", new Tuple<string, string, string, string, string>(

                                      "CONDITION", "Monitoring2", "Pupil(Rt) Reflex", null, null
                                      ));
            ret.Add("6000100001", new Tuple<string, string, string, string, string>(

                                      "발작", "대발작", null, null, null
                                      ));
            ret.Add("6000200001", new Tuple<string, string, string, string, string>(

                                      "발작", "소발작", null, null, null
                                      ));
            ret.Add("6000100003", new Tuple<string, string, string, string, string>(

                                   "111", "222", "330", null, null
                                   ));
            ret.Add("6000200004", new Tuple<string, string, string, string, string>(

                                      "111", "222", "331", null, null
                                      ));
            ret.Add("6000100005", new Tuple<string, string, string, string, string>(

                                  "111", "222", "332", "111", null
                                  ));
            ret.Add("6000200006", new Tuple<string, string, string, string, string>(

                                      "111", "222", "332", "222", null
                                      ));

            return ret;
        }
    }
}
