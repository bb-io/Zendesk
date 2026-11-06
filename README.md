# Blackbird.io Zendesk

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Zendesk is a customer service platform with ticketing, chat, help center content, and CRM capabilities. This app focuses on help center article localization and ticket workflows.

## Before setting up

Before you connect, make sure:

- You have a Zendesk account on the instance you want to connect to.
- Your Zendesk account has the right permissions. It is recommended to have the **admin role for Zendesk Guide**. If you want to set up event triggers, you need either the **admin role for Zendesk Support** or a [custom role](https://support.zendesk.com/hc/en-us/articles/4408882153882-Creating-custom-roles-and-assigning-agents#topic_cxn_hig_bd) with permission to manage webhooks.

## Connecting

This app supports two authentication methods: OAuth 2.0 and API Token. API Token is usually the easiest to set up and often more stable long-term.

### OAuth 2.0 authentication

1. Navigate to apps and search for Zendesk.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My client'.
4. Fill in the Base URL of the Zendesk instance you want to connect to. The format is `https://<instance name>.zendesk.com`.
5. Click _Authorize connection_.
6. Follow the instructions that Zendesk gives you, authorizing Blackbird.io to act on your behalf.
7. When you return to Blackbird, confirm that the connection has appeared and the status is _Connected_.

![Connecting](image/README/1692612174111.png)

> **Connecting to different brands**: If your Zendesk has multiple brands, you need to create a different connection for each separate brand. Brands are distinct by their base URL. You can find the base URL by going to *Settings > Account > Brand management > Brands* and viewing the *Subdomain* attribute of each brand. Use the Subdomain attribute in the Base URL connection input.

![1738163882548](image/README/1738163882548.png)

### API Token authentication

1. Navigate to apps and search for Zendesk.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My client'.
4. Fill in the Base URL of the Zendesk instance you want to connect to. The format is `https://<instance name>.zendesk.com`.
5. Fill `Email` with the email address of the Zendesk user you want to connect with.
6. Go to your Zendesk instance and navigate to *Zendesk Admin Center > Apps and integrations > APIs > API tokens* and create a new API token.
7. Give the 'Guide Admin' permission to the user account associated with your API token.
8. Copy the API token and paste it into the appropriate field in Blackbird.
9. Click _Connect_.
10. Confirm that the connection has appeared and the status is _Connected_.

## Actions

### Articles

- **Search articles** Search articles using filters.
    Advanced settings:
    - **Query**: Filter articles by search text.
    - **Category IDs**: Limit the search to one or more categories.
    - **Section IDs**: Limit the search to one or more sections.
    - **Label names**: Limit the search to articles with specific labels.
    - **Locale**: Filter results to a specific locale.
    - **Created after**: Include articles created after this date.
    - **Created before**: Include articles created before this date.
    - **Created at**: Include articles created on this date.
    - **Updated after**: Include articles updated after this date.
    - **Updated before**: Include articles updated before this date.
    - **Updated at**: Include articles updated on this date.
- **Get article metadata** Get metadata for a specific article.
- **Create article** Create a new article.
    Advanced settings:
    - **Content (HTML)**: Set the article content.
    - **Author**: Set the article author.
    - **Are comments disabled?**: Disable comments for the article.
    - **Is draft?**: Save the article as a draft.
    - **Is promoted?**: Mark the article as promoted.
    - **Position**: Set the article position.
    - **Notify subscribers?**: Notify subscribers about the new article.
- **Update article metadata** Update article metadata without changing translation content.
    Advanced settings:
    - **Permission group**: Update article visibility permissions.
    - **User segment**: Update the user segment for article visibility.
    - **Section**: Move the article to another section.
- **Archive article** Archive an article.
- **Add image to article content** Add an image at the bottom of an article.
- **Download article** Get the translatable content of an article as a file.
- **Get article ID from content file** Get the article ID embedded in the content file.
- **Upload article** Update an article translation or create it if it does not exist.
    Advanced settings:
    - **Is outdated**: Mark the translation as outdated.
    - **Article ID**: Specify the article ID instead of using file metadata.
- **Add label to article** Add a label to an article.
- **Delete label from article** Delete a label from an article.

![1751975102552](image/README/1751975102552.png)

### Categories

- **Search categories** Search categories, optionally filtered by missing translations.
    Advanced settings:
    - **Missing translation in**: Return only categories missing one or more locales.
- **Get category** Get information on a specific category.
- **Create category** Create a new category.
    Advanced settings:
    - **Description**: Set the category description.
- **Update category** Update a category.
- **Delete category** Delete a category.
- **Get category translation** Get a category translation for a specific locale.
- **Get category missing translations** Get the missing translations of a category.
- **Update category translation** Update a category translation or create it if it does not exist.
    Advanced settings:
    - **Title**: Set the translated title.
    - **Content**: Set the translated content.

### Sections

- **Search sections** Search sections, optionally filtered by category or missing translations.
    Advanced settings:
    - **Category ID**: Limit the search to a specific category.
- **Get section** Get information on a specific section.
- **Create section** Create a new section.
- **Update section** Update a section.
- **Delete section** Delete a section.
- **Get section translation** Get a section translation for a specific locale.
- **Get section missing translations** Get the missing translations of a section.
- **Update section translation** Update a section translation or create it if it does not exist.

### General

- **Search helpcenter languages** Get all activated help center languages.

### Tickets

- **Search tickets** Search tickets.
    Advanced settings:
    - **Status**: Filter tickets by status.
    - **Priority**: Filter tickets by priority.
- **Get ticket comments** Get comments for a ticket.
    Advanced settings:
    - **Newest first**: Sort comments from newest to oldest.
- **Get ticket** Get a specific ticket.
- **Create ticket** Create a new ticket.
    Advanced settings:
    - **Comment**: Add an initial ticket comment.
    - **Public**: Mark the comment as public.
    - **Subject**: Set the ticket subject.
- **Update ticket** Update a specific ticket.
- **Delete ticket** Delete a specific ticket.

### Debug

- **Debug** Debug the application.

## Events

### Articles

- **On article author changed** Triggers when an article author changes.
    Advanced settings:
    - **Brand ID**: Trigger only for a specific brand.
    - **Account ID**: Trigger only for a specific account.
    - **Article ID**: Trigger only for a specific article.
- **On article published** Triggers when an article is published.
    Advanced settings:
    - **Only source articles**: Trigger only when the published locale is the source locale.
    - **Required label**: Trigger only when the article contains this label.
    - **Language**: Trigger only for a specific locale.
- **On article subscription created** Triggers when an article subscription is created.
- **On article unpublished** Triggers when an article is unpublished.
- **On article vote created** Triggers when an article vote is created.
- **On article vote changed** Triggers when an article vote is changed.
- **On article vote removed** Triggers when an article vote is removed.
- **On article comment created** Triggers when an article comment is created.
- **On article comment changed** Triggers when an article comment is changed.
- **On article comment published** Triggers when an article comment is published.
- **On article comment unpublished** Triggers when an article comment is unpublished.

### Tickets

- **On ticket comment created** Triggers when a ticket comment is created.
    Advanced settings:
    - **Ignore if body contains**: Skip events when the comment body contains this text.
    - **Only public comments**: Trigger only for public comments.
    - **Ignore if web service comment**: Skip events created via the web service channel.

### Polling

- **On labels added to articles** Triggers when labels are added to matching articles. Optional **Deep search** flag queries articles by label name directly — enable this if label changes are not triggering the event on your Zendesk instance (some instances do not update `updated_at` when a label is added).
- **On new tickets added** Triggers when new tickets are created.

## Example

![example](image/README/1692615904702.png)
This example shows one of many use cases. Here, whenever an article is published we fetch the missing translations and retrieve the article as an HTML file. Then we create a new Lokalise project with the missing locales as the target languages and upload the article. Then we send a Slack message to notify the project manager.

## Demo video

How to Localize Zendesk Articles Automatically with Blackbird.io
[![Watch the video](https://img.youtube.com/vi/Nza3A0s81QY/0.jpg)](https://www.youtube.com/watch?v=Nza3A0s81QY)

## Missing features

Most content related actions exist. However, in the future we can add actions for:

- Article attachments
- Topics and Posts
- Comments
- Subscriptions
- Votes
- In depth tickets

Let us know if you're interested!

## Keep it from looping

Heads-up: using the On article published webhook together with the Upload article action can create an infinite loop. Upload article publishes the item, which re-triggers the webhook.

How to prevent it:

Set `draft = true` in Upload article so the upload is saved as a draft only.

Use a label-based workflow: configure the webhook to trigger only for items with a specific label (e.g., to_translate). After it fires, remove that label and add a new one (e.g., in_progress, then translate_done) to avoid re-triggering.
Sample:

![Sample](image/README/sample-prevent-loop.png)

## Feedback

Feedback to our implementation of Zendesk is always very welcome. Reach out to us using the established channels or create an issue.

<!-- end docs -->
