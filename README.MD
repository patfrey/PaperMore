# PaperMore - simple report generation for paperless-ngx

PaperMore is a small CLI tool for generating document reports from [paperless-ngx](https://github.com/paperless-ngx/paperless-ngx). It can output to csv, or pdf (powered by [QuestPDF](https://github.com/QuestPDF/QuestPDF)).

## Usage

Run the CLI with the required arguments:

```pwsh
PaperMore.exe --url <API_URL> --token <API_TOKEN> --format <csv|pdf> --path <OUTPUT_PATH>
```

### Options

- `--url`: URL of your paperless instance
- `--token`: API token for authentication
- `--format`: (optional) Output format (`csv` or `pdf`, default `pdf`)
- `--path`: Path to save the generated report
- `--blanklines`: Add blank lines to the PDF report, useful reserve space for newly added documents

> Note: BasicAuth login to the paperless api is not supported

## Contributing

Contributions are welcome! Please submit issues or pull requests via GitHub.
