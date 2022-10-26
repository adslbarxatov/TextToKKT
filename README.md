# TextToKKT v 9.6.9
> PCC: 0011FC54B1AD189F


Утилита сервис-инженера ККТ (контрольно-кассовая техника, 54-ФЗ)


#

## Описание

Инструмент позволяет:
- просматривать ***и сохранять / отправлять на печать*** инструкции по работе с ККТ;
- просматривать расшифровки кодов ошибок ККТ;
- определять срок жизни ФН в соответствии параметрами пользователя, о которых он зависит;
- определять корректность регистрационного номера ККТ и генерировать его при необходимости;
- определять соответствие модели ККТ версиям ФФД;
- получать настройки оператора фискальных данных (ОФД) по его ИНН или названию;
- получать описания и признаки обязательности TLV-тегов для текущего ФФД;
- получать описания команд нижнего уровня для фискальных регистраторов;
- вручную (без использования внешней клавиатуры) программировать текстовые данные в ККТ, имеющих только цифровую клавиатуру;
- автоматически определять модели ККТ и фискальных накопителей (ФН) по их заводским номерам, а также регионы РФ по кодам ИНН;
- просматривать распиновки кабелей ККТ – ПК, ККТ – ДЯ и ККТ – Принтер;
- определять тип штрих-кода и разбирать его содержимое (включая DataMatrix маркируемых товаров).

Версия для Windows имеет дополнительный модуль для работы с данными фискальных накопителей (см. далее).

&nbsp;



## Модуль FNReader

Компонент FNReader для Windows предназначен для чтения и обработки фискальных данных (ФД) из фискального накопителя (ФН).

Полное руководство пользователя доступно на [отдельной странице](https://github.com/adslbarxatov/TextToKKT/blob/master/FNReader.md).

&nbsp;



## Требования

- Windows 7 или новее;
- [Microsoft .NET Framework 4.8](https://go.microsoft.com/fwlink/?linkid=2088631);
- [Microsoft Visual C++ 2015 – 2022 redistributable](https://aka.ms/vs/17/release/vc_redist.x86.exe) *(для модуля FNReader)*.

Язык интерфейса: ru_ru.

Список поддерживаемых TLV-тегов можно посмотреть [здесь](https://github.com/adslbarxatov/TextToKKT/blob/master/src/FNReader/FNEnums.h).

#

- Android-версия доступна на [Google play](https://play.google.com/store/apps/details?id=com.RD_AAOW.TextToKKT); для неё потребуется ОС Android 5.0 или новее;
- Android-версия доступна в [специальном релизе](https://github.com/adslbarxatov/TextToKKT/releases/latest) для Android 4.2 и новее или устройств без Play market;
- Android-версия доступна также на платформе [AppGallery](https://appgallery.cloud.huawei.com/ag/n/app/C102663035?channelId=GitHub&id=f4e62031e4b84ebb937a8b75c08fc543&s=E90E5D1DB649783589F6F3EA42475CFECAABFFF996E407E61BACB2268DB9867C&detailType=0&v=&callType=AGDLINK&installType=0000).
- Приложение также доступно в [GetApps](https://global.app.mi.com/details?id=texttokkt.xiaomi).

&nbsp;



## Требования к оборудованию для работы модуля FNReader

Работа программы протестирована на всех моделях ФН из реестра ФНС, включая вышедшие из обращения и МГМ.
При соблюдении производителями существующего аппаратного протокола чтения данных работа с будущими моделями ФН также будет возможна.

Работа программы возможна при наличии следующих аппаратных адаптеров:

- COM-UART переходник для подключения ФН к разъёму COM (RS-232) компьютера;

- USB-VCOM-UART переходник для подключения к разъёму USB компьютера. В этом случае потребуется установить драйвера устройства;
инструкции и ссылки для их установки предоставляются производителями таких переходников.

**Обращаем внимание, что срок действия каждого релиза модуля ограничен с целью устранения устаревающих версий
программы и обеспечения постоянного соответствия актуальным изменениям в ФФД. Срок указан в заголовке окна компонента.
По его истечении останутся доступными функции анализа ранее считанных данных. По истечению срока для работы с новыми данными
необходимо получить новый экземпляр компонента**

&nbsp;



## [Политика разработки и EULA](https://adslbarxatov.github.io/ADP/ru)

Данная Политика (ADP), её положения, заключение, EULA и способы применения
описывают общие правила, которым мы следуем во всех наших процессах разработки, вышедших в релиз приложениях
и реализованных идеях.
***Обязательна к ознакомлению для всех участников и пользователей перед использованием любого из продуктов лаборатории.
Загружая их, вы соглашаетесь и принимаете эту Политику!***
