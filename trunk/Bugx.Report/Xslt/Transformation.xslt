<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output omit-xml-declaration="yes" encoding="utf-8" method="html"/>
  <xsl:template match="/bugx">
    <xsl:text disable-output-escaping="yes">
      <![CDATA[<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strinct//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">]]>
    </xsl:text>
    <html>
      <head>
        <title>Error: <xsl:value-of select="url"/></title>
        <style type="text/css">
          /*<![CDATA[*/
          html, body
          {
            margin:0px;
            padding:0px;
            background:#F0F3F8;
            font:normal 10 px verdana;
          }
          fieldset
          {
            background:#E0DFFF;
            -moz-border-radius:7px;
            margin-top:10px;
          }
          legend
          {
            color:5B79CC;
            background:#FFFFE1;
            border:solid 1px #000;
          }
          th
          {
            width:200px;
            background:#9A99DF;
            border:solid 1px #000;
            text-align:left;
          }
          td
          {
          }
          .StackTrace div
          {
            font:normal 10pt "courier new";
            background:#fff;
            padding:0px 5px;
          }
          .StackTrace .NotRelevant, .StackTrace .NotRelevant *
          {
            color:#808080 !important;
          }
          .StackTrace .namespace, .StackTrace .arguments, .StackTrace .method
          {
            color:#000;
          }
          .StackTrace .class
          {
            color:#2B91AF;
          }
          .StackTrace .location
          {
            color:#008000;
            font:normal 8pt "courier new";
          }
          /*]]>*/
        </style>
      </head>
      <body>
        Error on 
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="url"/>
          </xsl:attribute>
          <xsl:value-of select="url"/>
        </a>
        (<xsl:value-of select="@date"/>)
        <fieldset>
          <legend>Complete error description:</legend>
          <xsl:if test="exception">
            <fieldset class="StackTrace">
              <legend>
                <xsl:value-of select="exception/@type"/>:
                <xsl:value-of select="exception/@message"/>
              </legend>
              <xsl:for-each select="stackTrace/trace">
                <div>
                  <xsl:if test="@relevant=0">
                    <xsl:attribute name="class">NotRelevant</xsl:attribute>
                  </xsl:if>
                  <xsl:apply-templates />
                </div>
              </xsl:for-each>
            </fieldset>
          </xsl:if>
          <xsl:if test="queryString/*">
            <fieldset>
              <legend>QueryString:</legend>
              <table>
                <xsl:for-each select="queryString/*">
                  <tr>
                    <th class="Variable" title="System.String">
                      <xsl:value-of select="name()"/>
                    </th>
                    <td>
                      <code>
                        <xsl:value-of select="."/>
                      </code>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </fieldset>
          </xsl:if>
          <xsl:if test="form/*">
            <fieldset>
              <legend>Form:</legend>
              <table>
                <xsl:for-each select="form/*">
                  <tr>
                    <th class="Variable" title="System.String">
                      <xsl:value-of select="name()"/>
                    </th>
                    <td>
                    <code>
                      <xsl:value-of select="."/>
                    </code>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </fieldset>
          </xsl:if>
          <xsl:if test="sessionVariables/*">
            <fieldset>
              <legend>Session:</legend>
              <table>
                <tr>
                  <th>Code page:</th>
                  <td>
                    <xsl:value-of select="sessionVariables/@codePage"/>
                  </td>
                </tr>
                <tr>
                  <th>Mode:</th>
                  <td>
                    <xsl:value-of select="sessionVariables/@mode"/>
                  </td>
                </tr>
                <tr>
                  <th>Count:</th>
                  <td>
                    <xsl:value-of select="count(sessionVariables/add)"/>
                  </td>
                </tr>
                <xsl:if test="sessionVariables/add">
                  <xsl:for-each select="sessionVariables/add">
                    <tr>
                      <th class="Variable">
                        <xsl:attribute name="title">
                          <xsl:value-of select="@type"/>
                        </xsl:attribute>
                        <xsl:value-of select="@name"/>
                      </th>
                      <td>
                        <xsl:choose>
                          <xsl:when test="@value">
                            <code>
                              <xsl:value-of select="@value"/>
                            </code>
                          </xsl:when>
                          <xsl:when test=".">
                            <code>(object)</code>
                          </xsl:when>
                          <xsl:otherwise>
                            <code class="Error">No data has been serialized for this object</code>
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                  </xsl:for-each>
                </xsl:if>
              </table>
            </fieldset>
          </xsl:if>
          <xsl:if test="cacheVariables/*">
            <fieldset>
              <legend>Cache:</legend>
              <table>
                <tr>
                  <th>Count:</th>
                  <td>
                    <xsl:value-of select="count(cacheVariables/add)"/>
                  </td>
                </tr>
                <xsl:for-each select="cacheVariables/add">
                  <tr>
                    <th class="Variable">
                      <xsl:attribute name="title">
                        <xsl:value-of select="@type"/>
                      </xsl:attribute>
                      <xsl:value-of select="@name"/>
                    </th>
                    <td>
                      <xsl:choose>
                        <xsl:when test="@value">
                          <code>
                            <xsl:value-of select="@value"/>
                          </code>
                        </xsl:when>
                        <xsl:when test=".">
                          <code>(object)</code>
                        </xsl:when>
                        <xsl:otherwise>
                          <code class="Error">No data has been serialized for this object</code>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </fieldset>
          </xsl:if>
          <xsl:if test="user">
            <fieldset>
              <legend>User:</legend>
              <table>
                <tr>
                  <th>Name:</th>
                  <td>
                    <xsl:value-of select="user/@name"/>
                  </td>
                </tr>
                <tr>
                  <th>Authentication:</th>
                  <td>
                    <xsl:value-of select="user/@authenticationType"/>
                  </td>
                </tr>
              </table>
            </fieldset>
          </xsl:if>
          <fieldset>
            <legend>Server:</legend>
            <table>
              <tr>
                <th>Machine Name:</th>
                <td>
                  <xsl:value-of select="machineName"/>
                </td>
              </tr>
              <tr>
                <th>Script Timeout:</th>
                <td>
                  <xsl:value-of select="scriptTimeout"/>
                </td>
              </tr>
            </table>
          </fieldset>
          <xsl:if test="headers/*">
            <fieldset>
              <legend>Headers:</legend>
              <table>
                <xsl:for-each select="headers/*">
                  <tr>
                    <th class="Variable" title="System.String">
                      <xsl:value-of select="name()"/>
                    </th>
                    <td>
                      <code>
                        <xsl:value-of select="."/>
                      </code>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </fieldset>
          </xsl:if>
        </fieldset>
      </body>
    </html>
  </xsl:template>
<!-- Stacktrace transformation -->
  <xsl:template match="trace/namespace">
    <span class="namespace">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="class">
    <span class="class">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="trace/method">
    <span class="method">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="trace/arguments">
    <span class="arguments">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="trace/location">
    <span class="location">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
</xsl:stylesheet>