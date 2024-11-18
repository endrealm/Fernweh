﻿using Core.Content;

namespace Core.Utils;

public interface ILoadable
{
    void Load(ContentLoader content);
}