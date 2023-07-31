using Apps.Zendesk.Dtos;
using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses
{
    public class Translation
    {
        [Display("Translation ID")]
        public string Id { get; set; }

        [Display("Public URL")]
        public string html_url { get; set; }

        [Display("Source ID")]
        public string SourceId { get; set; }

        [Display("Source type")]
        public string SourceType { get; set; }

        public string Locale { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        [Display("Is draft?")]
        public bool Draft { get; set; }

        [Display("Is hidden?")]
        public bool Hidden { get; set; }

        [Display("Is outdated?")]
        public bool Outdated { get; set; }

        [Display("Created at")]
        public DateTime CreatedAt { get; set; }

        [Display("Updated at")]
        public DateTime UpdatedAt { get; set; }

        public static Translation? FromDto(TranslationDto? dto)
        {
            if (dto == null) return null;
            return new Translation
            {
                Id = dto.Id.ToString(),
                html_url = dto.html_url,
                Draft = dto.Draft,
                Hidden = dto.Hidden,
                Outdated = dto.Outdated,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                Title = dto.Title,
                Locale = dto.Locale,
                Body = dto.Body,
                SourceId = dto.SourceId,
                SourceType = dto.SourceType,
            };
        }
    }
}
