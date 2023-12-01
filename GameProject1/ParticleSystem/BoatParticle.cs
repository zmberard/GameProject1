﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameProject1
{
    public class BoatParticle : ParticleSystem
    {
        /// <summary>
        /// requirement 3: sprite recoloring
        /// </summary>
        Color[] colors = new Color[]
        {
            Color.Gray,
            Color.DimGray,
            Color.DarkGray,
            Color.LightGray,
            Color.DarkSlateGray,
            Color.White,
            Color.AntiqueWhite
        };

        Color color;

        public BoatParticle(Game game, int maxExplosions) : base(game, maxExplosions * 25) { }

        protected override void InitializeConstants()
        {
            textureFilename = "circle";
            minNumParticles = 4;
            maxNumParticles = 6;
            //requirement 4 met alpha blending
            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }
        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(40, 200);

            var lifetime = RandomHelper.NextFloat(0.5f, 1.0f);

            var acceleration = -velocity / lifetime;
            //requirement 2: sprite rotations
            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);

            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

            var scale = RandomHelper.NextFloat(4, 6);

            p.Initialize(where, velocity, acceleration, color, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity, scale: scale);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;

            particle.Scale = .1f + .25f * normalizedLifetime;


        }

        public void PlaceFirework(Vector2 where)
        {
            color = colors[RandomHelper.Next(colors.Length)];
            AddParticles(where);
        }
    }
}
