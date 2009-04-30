A mettre dans le web.config:

<configuration>
    <configSections>
        <sectionGroup name="bugx">
          <section name="watermark" type="System.Configuration.NameValueFileSectionHandler, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
    </configSections>
    <!-- Optional parameters -->
    <bugx>
        <watermark>
          <add key="Enable" value="true/false"/>
          <add key="Text" value="Pre-production Environment"/>
        </watermark>
        <watermark.menu>
          <add key="Google" value="http://www.google.com" />
        </watermark.menu>
    </bugx>

    <system.web>
      <httpModules>
        <add name="BugxWatermark" type="Bugx.Watermark.EnvironmentWatermark, Bugx.Watermark"/>
      </httpModules>
    </system.web>
    
    <!-- IIS 7 Section -->
    <system.webServer>
      <modules>
        <add name="BugxWatermark" type="Bugx.Watermark.EnvironmentWatermark, Bugx.Watermark"/>
      </modules>
    </system.webServer>
</configuration>
