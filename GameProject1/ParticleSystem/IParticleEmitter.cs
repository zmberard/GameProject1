﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1
{

    public interface IParticleEmmitter
    {
        public Vector2 Position { get; }

        public Vector2 Velocity { get; }
    }

}
