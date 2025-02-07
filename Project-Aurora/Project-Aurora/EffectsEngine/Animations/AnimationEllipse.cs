﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Aurora.EffectsEngine.Animations
{
    public class AnimationEllipse : AnimationFrame
    {
        [Newtonsoft.Json.JsonProperty]
        private float _radius_x = 0.0f;
        [Newtonsoft.Json.JsonProperty]
        private float _radius_y = 0.0f;
        [Newtonsoft.Json.JsonProperty]
        private PointF _center = new PointF();

        public float RadiusHorizontal { get { return _radius_x; } }
        public float RadiusVertical { get { return _radius_y; } }
        public PointF Center { get { return _center; } }


        public AnimationFrame SetRadiusHorizontal(float radius)
        {
            _radius_x = radius;
            _dimension = new RectangleF(_center.X - _radius_x, _center.Y - _radius_y, 2.0f * _radius_x, 2.0f * _radius_y);
            _invalidated = true;

            return this;
        }

        public AnimationFrame SetRadiusVertical(float radius)
        {
            _radius_y = radius;
            _dimension = new RectangleF(_center.X - _radius_x, _center.Y - _radius_y, 2.0f * _radius_x, 2.0f * _radius_y);
            _invalidated = true;

            return this;
        }

        public AnimationFrame SetCenter(PointF center)
        {
            _center = center;
            _dimension = new RectangleF(_center.X - _radius_x, _center.Y - _radius_y, 2.0f * _radius_x, 2.0f * _radius_y);
            _invalidated = true;

            return this;
        }

        public AnimationEllipse()
        {
            _radius_x = 0;
            _radius_y = 0;
            _center = new PointF(0, 0);
            _dimension = new RectangleF(_center.X - _radius_x, _center.Y - _radius_y, 2.0f * _radius_x, 2.0f * _radius_y);
            _color = Utils.ColorUtils.GenerateRandomColor();
            _width = 1;
            _duration = 0.0f;
        }

        public AnimationEllipse(AnimationFrame frame, float radiusX, float radiusY) : base(frame)
        {
            _radius_x = radiusX;
            _radius_y = radiusY;
            _center = new PointF(_dimension.X + _radius_x, _dimension.Y + _radius_y);
        }

        public AnimationEllipse(AnimationEllipse animationEllipse) : base(animationEllipse)
        {
            _radius_x = animationEllipse._radius_x;
            _radius_y = animationEllipse._radius_y;
            _center = animationEllipse._center;
        }

        public AnimationEllipse(Rectangle dimension, Color color, int width = 1, float duration = 0.0f) : base(dimension, color, width, duration)
        {
            _radius_x = dimension.Width / 2.0f;
            _radius_y = dimension.Height / 2.0f;
            _center = new PointF(dimension.X + _radius_x, dimension.Y + _radius_y);
        }

        public AnimationEllipse(RectangleF dimension, Color color, int width = 1, float duration = 0.0f) : base(dimension, color, width, duration)
        {
            _radius_x = dimension.Width / 2.0f;
            _radius_y = dimension.Height / 2.0f;
            _center = new PointF(dimension.X + _radius_x, dimension.Y + _radius_y);
        }

        public AnimationEllipse(PointF center, float x_axis, float y_axis, Color color, int width = 1, float duration = 0.0f)
        {
            _radius_x = x_axis;
            _radius_y = y_axis;
            _center = new PointF(center.X + _radius_x, center.Y + _radius_y);
            _dimension = new RectangleF(_center.X - _radius_x, _center.Y - _radius_y, 2.0f * _radius_x, 2.0f * _radius_y);
            _color = color;
            _width = width;
            _duration = duration;
        }

        public AnimationEllipse(float x, float y, float x_axis, float y_axis, Color color, int width = 1, float duration = 0.0f)
        {
            _radius_x = x_axis;
            _radius_y = y_axis;
            _center = new PointF(x + _radius_x, y + _radius_y);
            _dimension = new RectangleF(_center.X - _radius_x, _center.Y - _radius_y, 2.0f * _radius_x, 2.0f * _radius_y);
            _color = color;
            _width = width;
            _duration = duration;
        }

        protected override void virtUpdate()
        {
            base._center = new PointF(_center.X * Scale, _center.Y * Scale);

            base.virtUpdate();
        }

        public override void Draw(Graphics g)
        {
            if (_pen == null || _invalidated)
            {
                _pen = new Pen(_color);
                _pen.Width = _width;
                _pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                _pen.ScaleTransform(Scale, Scale);

                virtUpdate();
                _invalidated = false;
            }

            g.ResetTransform();
            g.DrawEllipse(_pen, _dimension);
        }

        public override AnimationFrame BlendWith(AnimationFrame otherAnim, double amount)
        {
            if (!(otherAnim is AnimationEllipse))
            {
                throw new FormatException("Cannot blend with another type");
            }

            amount = GetTransitionValue(amount);

            RectangleF newrect = new RectangleF((float)CalculateNewValue(_dimension.X, otherAnim._dimension.X, amount),
                (float)CalculateNewValue(_dimension.Y, otherAnim._dimension.Y, amount),
                (float)CalculateNewValue(_dimension.Width, otherAnim._dimension.Width, amount),
                (float)CalculateNewValue(_dimension.Height, otherAnim._dimension.Height, amount)
                );

            int newwidth = (int)CalculateNewValue(_width, otherAnim._width, amount);
            float newAngle = (float)CalculateNewValue(_angle, otherAnim._angle, amount);

            return new AnimationEllipse(newrect, Utils.ColorUtils.BlendColors(_color, otherAnim._color, amount), newwidth).SetAngle(newAngle);
        }

        public override AnimationFrame GetCopy()
        {
            return new AnimationEllipse(this);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AnimationEllipse)obj);
        }

        public bool Equals(AnimationEllipse p)
        {
            return _color.Equals(p._color) &&
                _dimension.Equals(p._dimension) &&
                _width.Equals(p._width) &&
                _duration.Equals(p._duration) &&
                _angle.Equals(p._angle);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _color.GetHashCode();
                hash = hash * 23 + _dimension.GetHashCode();
                hash = hash * 23 + _width.GetHashCode();
                hash = hash * 23 + _duration.GetHashCode();
                hash = hash * 23 + _angle.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"AnimationEllipse [ Color: {_color.ToString()} Dimensions: {_dimension.ToString()} Width: {_width} Duration: {_duration} Angle: {_angle} ]";
        }
    }
}
