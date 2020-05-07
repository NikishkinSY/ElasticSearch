﻿using ES.Infrastructure.ElasticSearch.Entities;
using Nest;

namespace ES.Infrastructure.ElasticSearch
{
    public static class CreateManagementIndex
    {
        public static CreateIndexDescriptor CreateManagementIndexSettingAndMapping(this CreateIndexDescriptor indexDescriptor)
        {
            indexDescriptor.Settings(s => s
                    .Setting("index.codec", "best_compression")
                    .Analysis(a =>
                        a.TokenFilters(t => t
                                .Stemmer("english_stemmer", st => st
                                    .Language("english")
                                )
                                .Stop("custom_stop", stop => stop
                                    .StopWords(new string[] { "and", "or", "into", "the" })
                                )
                            )
                            .Analyzers(an => an
                                .Custom("main_analyzer", ca => ca
                                    .Tokenizer("standard")
                                    .Filters("english_stemmer", "custom_stop")
                                )
                            )
                    )
                )
                .Map<ManagementES>(m => m
                    .Properties(pr => pr
                        .Scalar(s => s.Id)
                        .Text(t => t
                            .Name(n => n.Name)
                            .Analyzer("main_analyzer")
                        )
                        .Keyword(k => k
                            .Name(n => n.Market)
                        )
                        .Keyword(k => k
                            .Name(n => n.State)
                        )
                    )
                );

            return indexDescriptor;
        }
    }
}
