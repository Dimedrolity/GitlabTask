# GitlabTask
Консольное приложения для просмотра Gitlab коммитов

Приложение содержит конфиг-файл (appsettings.json). В нем содержится:
- Доменное имя gitlab'а. Например, "gitlab.com".
- Персональный токен доступа. Если вам не нужен токен для доступа к коммитам, то необходимо указать null.
- Список отслеживаемых проектов. Обязательно нужно указать Project ID (скопировать с Gitlab'а) и желательно указать название проекта (например, в сокращенном варианте).
- Шаблоны регулярных выражений для заголовков коммитов, которые не нужно выводить.


По умолчанию выводятся коммиты за последний день. Команду коммитов можно конфигурировать по временному интервалу - показывать коммиты за последние X часов и X дней.

