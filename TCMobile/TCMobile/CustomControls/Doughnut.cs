using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin;
using Xamarin.Forms;

namespace TCMobile.CustomControls
{
    class Doughnut
    {
        public Grid CompletionChart(string label,float perc_complete, float perc_incomplete)
        {
            
            //perc_complete = (float)Math.Ceiling(perc_complete);
            Grid container = new Grid()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                Margin = 0
            };

            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(15) });
            container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            var percLabel = new Label
            {

                Text = perc_complete.ToString() + "%",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                FontSize = 12
            };
            var completeLabel = new Label
            {
                Text = label,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.End,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                TextColor = Color.Black,
                FontSize = 14

            };

            List<Microcharts.ChartEntry> entries = new List<ChartEntry>
                {
                    new ChartEntry(perc_complete)
                    {

                        Color = SKColor.Parse(Constants.MenuBackgroundColour)
                    },
                    new ChartEntry(perc_incomplete)
                    {
                        Color = SKColor.Parse(Constants.HeaderColour)
                    }

            };

            ChartView chartView = null;
            var completionChart = new DonutChart() {
                Entries = entries,
                IsAnimated = false,
                HoleRadius =0.7f,
                Margin = 0
                
            };
            chartView = new ChartView
            {
                Chart = completionChart,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 40,
                Margin = 0                

            };

            container.Children.Add(chartView,0,0);
            container.Children.Add(percLabel, 0, 0);
            container.Children.Add(completeLabel, 0, 1);
            return container;
        }
          
        }
    }

