﻿using AutoMapper;
using Toolkit.Interfaces;

namespace Toolkit.Mapper;

internal class NestedMapper : IGenericMapper, INestedMapper
{
    private AutoMapper.Mapper _Mapper;
    private MapperConfigurationExpression _Expression;

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        CreateMap();
        return _Mapper.Map<TDestination>(source);
    }

    public IGenericMapper Build<TSource, TDestination>()
    {
        return Nest<TSource, TDestination>() as IGenericMapper;
    }

    public INestedMapper Nest<TSource, TDestination>()
    {
        CreateExpression<TSource, TDestination>();
        return this;
    }

    private void CreateExpression<TSource, TDestination>()
    {
        if (_Expression == null)
            _Expression = new MapperConfigurationExpression();
        _Expression.CreateMap<TSource, TDestination>();
    }

    private void CreateMap()
    {
        if (_Mapper != null)
            return;
        var cfg = new MapperConfiguration(_Expression);
        _Mapper = new AutoMapper.Mapper(cfg);
    }
}