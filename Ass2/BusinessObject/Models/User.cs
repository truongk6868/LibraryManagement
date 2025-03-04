﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int? ReaderId { get; set; }

    public virtual Reader? Reader { get; set; }
}
