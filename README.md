# TextToKKT v 8.4.1
> PCC: 000EE08BC9D5C3D3


Утилита сервис-инженера ККТ (контрольно-кассовая техника, 54-ФЗ)


#

## Описание

Инструмент позволяет:
- просматривать инструкции по работе с ККТ;
- просматривать расшифровки кодов ошибок ККТ;
- определять срок жизни ФН в соответствии параметрами пользователя, о которых он зависит;
- определять корректность регистрационного номера ККТ и генерировать его при необходимости;
- определять соответствие модели ККТ версиям ФФД 1.1 и 1.2;
- получать настройки оператора фискальных данных (ОФД) по его ИНН или названию;
- получать описания и ***признаки обязательности*** TLV-тегов для текущего ФФД;
- получать описания команд нижнего уровня для фискальных регистраторов;
- вручную (без использования внешней клавиатуры) программировать текстовые данные в ККТ, имеющих только цифровую клавиатуру;
- автоматически определять модели ККТ и фискальных накопителей (ФН) по их заводским номерам, а также регионы РФ по кодам ИНН;
- просматривать распиновки кабелей ККТ – ПК, ККТ – ДЯ и ККТ – Принтер;
- определять тип штрих-кода и разбирать его содержимое (***включая DataMatrix маркируемых товаров***).

Версия для Windows теперь имеет дополнительный модуль для работы с данными фискальных накопителей (см. далее).

&nbsp;



## Модуль FNReader

Компонент FNReader для Windows предназначен для чтения и обработки фискальных данных (ФД) из фискального накопителя (ФН).

Полное руководство пользователя теперь доступно на [отдельной странице](https://github.com/adslbarxatov/TextToKKT/blob/master/FNReader.md).

&nbsp;



## Требования

- Windows XP or newer / или новее;
- [Microsoft .NET Framework 4.0](https://www.microsoft.com/en-us/download/details.aspx?id=17718);
- [Microsoft VC++ 2010 redistributables](https://www.microsoft.com/en-us/download/details.aspx?id=26999) *(для модуля FNReader)*.

Язык интерфейса: ru_ru.

Список поддерживаемых TLV-тегов можно посмотреть [здесь](https://github.com/adslbarxatov/TextToKKT/blob/master/src/FNReader/FNEnums.h).

#

- Android-версия доступна на [Google play](https://play.google.com/store/apps/details?id=com.RD_AAOW.TextToKKT);
для неё потребуется ОС Android 5.0 или новее;
- Android-версия доступна в [специальном релизе](https://github.com/adslbarxatov/TextToKKT/releases/latest) для Android 4.x
или устройств без Play market;
- Android-версия доступна также на платформе AppGallery.

&nbsp;



## Требования к оборудованию для работы модуля FNReader

Работа программы протестирована на всех моделях ФН из реестра ФНС.
При соблюдении производителями существующего аппаратного протокола чтения данных работа с будущими моделями ФН также будет возможна.
**Поддержка версий ФФД 1.1 и более новых на данный момент не гарантируется**.

Работа программы возможна при наличии следующих аппаратных адаптеров:

- COM-UART переходник для подключения ФН к разъёму COM (RS-232) компьютера;

- USB-VCOM-UART переходник для подключения к разъёму USB компьютера. В этом случае потребуется установить драйвера устройства;
инструкции и ссылки для их установки предоставляются производителями таких переходников.

**Обращаем внимание, что срок действия каждого релиза модуля ограничен с целью устранения устаревающих версий
программы и обеспечения постоянного соответствия актуальным изменениям в ФФД. Срок указан в заголовке окна компонента.
По его истечении останутся доступными функции анализа ранее считанных данных. По истечению срока для работы с новыми данными
необходимо получить новый экземпляр компонента**

&nbsp;



## [Политика разработки и EULA](https://adslbarxatov.github.io/ADP)

Данная Политика (ADP), её положения, заключение, EULA и способы применения
описывают общие правила, которым мы следуем во всех наших процессах разработки, вышедших в релиз приложениях
и реализованных идеях.
***Обязательна к ознакомлению для всех участников и пользователей перед использованием любого из продуктов лаборатории.
Загружая их, вы соглашаетесь и принимаете эту Политику!***
