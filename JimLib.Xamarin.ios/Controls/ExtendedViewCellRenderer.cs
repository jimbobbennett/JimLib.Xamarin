using System;
using System.Drawing;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedViewCell), typeof(ExtendedViewCellRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ExtendedViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableView tv)
        {
            var extendedCell = (ExtendedViewCell) item;
            var cell = base.GetCell(item, tv);
            if (cell != null)
            {
                cell.SelectionStyle = extendedCell.HighlightSelection ? UITableViewCellSelectionStyle.Default : UITableViewCellSelectionStyle.None;

                cell.BackgroundColor = extendedCell.BackgroundColor.ToUIColor();
                cell.SeparatorInset = new UIEdgeInsets((float) extendedCell.SeparatorPadding.Top, (float) extendedCell.SeparatorPadding.Left,
                    (float) extendedCell.SeparatorPadding.Bottom, (float) extendedCell.SeparatorPadding.Right);
                if (extendedCell.ShowDisclosure)
                {
                    cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    if (!string.IsNullOrEmpty(extendedCell.DisclosureImage))
                    {
                        var detailDisclosureButton = UIButton.FromType(UIButtonType.Custom);
                        detailDisclosureButton.SetImage(UIImage.FromBundle(extendedCell.DisclosureImage), UIControlState.Normal);
                        detailDisclosureButton.SetImage(UIImage.FromBundle(extendedCell.DisclosureImage), UIControlState.Selected);
                        detailDisclosureButton.Frame = new RectangleF(0f, 0f, 30f, 30f);
                        detailDisclosureButton.TouchUpInside += (sender, e) =>
                            {
                                try
                                {
                                    var index = tv.IndexPathForCell(cell);
                                    tv.SelectRow(index, true, UITableViewScrollPosition.None);
                                    tv.Source.RowSelected(tv, index);
                                }
                                catch (You_Should_Not_Call_base_In_This_Method)
                                {
                                    Console.Write("Xamarin Forms Labs Weird stuff : You_Should_Not_Call_base_In_This_Method happend");
                                }
                            };
                        cell.AccessoryView = detailDisclosureButton;
                    }
                }
            }
            if (!extendedCell.ShowSeparator)
                tv.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tv.SeparatorColor = extendedCell.SeparatorColor.ToUIColor();
            return cell;
        }
    }
}