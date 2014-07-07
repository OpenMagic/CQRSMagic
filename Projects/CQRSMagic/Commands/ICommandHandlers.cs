﻿using System;
using System.Collections.Generic;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Commands
{
    public interface ICommandHandlers
    {
        Func<ICommand, IEnumerable<IEvent>> GetCommandHandlerFor(ICommand command);
    }
}