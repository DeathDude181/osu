// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using OpenTK;
using OpenTK.Graphics;

namespace osu.Game.Overlays.Toolbar
{
    public class ToolbarNotificationButton : ToolbarOverlayToggleButton
    {
        protected override Anchor TooltipAnchor => Anchor.TopRight;

        public BindableInt NotificationCount = new BindableInt();

        private readonly CountCircle countDisplay;

        public ToolbarNotificationButton()
        {
            Icon = FontAwesome.fa_bars;
            TooltipMain = "Notifications";
            TooltipSub = "Waiting for 'ya";

            Add(countDisplay = new CountCircle
            {
                Height = 16,
                RelativePositionAxes = Axes.Both,
                Origin = Anchor.Centre,
                Position = new Vector2(0.7f, 0.25f),
            });
        }

        [BackgroundDependencyLoader(true)]
        private void load(NotificationOverlay notificationOverlay)
        {
            StateContainer = notificationOverlay;

            if (notificationOverlay != null)
                NotificationCount.BindTo(notificationOverlay.UnreadCount);

            NotificationCount.ValueChanged += count =>
            {
                countDisplay.FadeTo(count == 0 ? 0 : 1, 200, Easing.OutQuint);
                countDisplay.Count = count;
            };
        }

        private class CountCircle : CompositeDrawable
        {
            private readonly OsuSpriteText countText;
            private readonly Circle circle;
            private readonly Circle expandCircle;

            private readonly Container elasticContainer;

            private int count;

            public int Count
            {
                get { return count; }
                set
                {
                    if (count == value)
                        return;

                    if (value > count)
                    {
                        circle.FlashColour(Color4.White, 600, Easing.OutQuint);

                        if (count == 0 && expandCircle.Alpha < 0.1f)
                            expandCircle.FadeTo(0.8f).ScaleTo(1).Then().ScaleTo(50, 500).FadeOut(300);

                        elasticContainer.ScaleTo(1.1f).Then().ScaleTo(1, 600, Easing.OutElastic);
                    }

                    count = value;
                    if (count > 0)
                        countText.Text = value.ToString("#,0");
                }
            }

            public CountCircle()
            {
                AutoSizeAxes = Axes.X;

                InternalChildren = new Drawable[]
                {
                    expandCircle = new Circle
                    {
                        Alpha = 0,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        BypassAutoSizeAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Red
                    },
                    elasticContainer = new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.X,
                        RelativeSizeAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            circle = new Circle
                            {
                                Blending = BlendingMode.Additive,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.Red
                            },
                            countText = new OsuSpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Y = -1,
                                TextSize = 14,
                                Padding = new MarginPadding(5),
                                Colour = Color4.White,
                                UseFullGlyphHeight = true,
                                Font = "Exo2.0-Bold",
                            }
                        }
                    }
                };
            }
        }
    }
}
